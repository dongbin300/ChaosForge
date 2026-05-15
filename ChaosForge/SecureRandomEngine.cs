using System.Security.Cryptography;

namespace ChaosForge
{
	/// <summary>
	/// Provides a high-entropy cryptographic-style random number generator
	/// based on entropy pooling, SHA-256 whitening, and counter-driven output generation.
	/// </summary>
	public class SecureRandomEngine
	{
		/// <summary>
		/// Internal entropy pool used for entropy accumulation and reseeding. 
		/// </summary>
		private readonly EntropyPool _pool = new();

		/// <summary>
		/// Internal cryptographic key material derived from the entropy pool.
		/// </summary>
		private byte[] _key = default!;
		/// <summary>
		/// Internal output counter used to generate unique output blocks.
		/// </summary>
		private ulong _counter;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRandomEngine"/> class
		/// and performs an initial reseed operation.
		/// </summary>
		public SecureRandomEngine()
		{
			Reseed();
		}

		/// <summary>
		/// Reseeds the generator by collecting fresh entropy,
		/// mixing it into the entropy pool, and regenerating
		/// the internal key material.
		/// </summary>
		public void Reseed()
		{
			byte[] entropy = EntropyCollector.Collect();

			_pool.Mix(entropy);

			_key = SHA256.HashData(_pool.Snapshot());

			_counter = BitConverter.ToUInt64(_key, 0);
		}

		/// <summary>
		/// Generates a cryptographically-inspired sequence of random bytes.
		/// The output is produced by hashing the internal key together
		/// with a continuously incrementing counter value.
		/// </summary>
		/// <param name="length">
		/// The number of random bytes to generate.
		/// </param>
		/// <returns>
		/// A byte array containing the generated random data.
		/// </returns>
		public byte[] NextBytes(int length)
		{
			List<byte> output = [];

			while (output.Count < length)
			{
				byte[] counterBytes = BitConverter.GetBytes(_counter);

				byte[] input = new byte[_key.Length + counterBytes.Length];

				Buffer.BlockCopy(_key, 0, input, 0, _key.Length);
				Buffer.BlockCopy(counterBytes, 0, input, _key.Length, counterBytes.Length);

				byte[] block = SHA256.HashData(input);

				output.AddRange(block);

				_counter++;
			}

			// Periodic reseed
			if (_counter % 1024 == 0)
			{
				Reseed();
			}

			return [.. output.Take(length)];
		}

		/// <summary>
		/// Generates a non-negative random 32-bit integer.
		/// </summary>
		/// <returns>
		/// A pseudo-random positive integer value.
		/// </returns>
		public int NextInt()
		{
			byte[] bytes = NextBytes(4);

			return Math.Abs(BitConverter.ToInt32(bytes, 0));
		}

		/// <summary>
		/// Generates a random floating-point value between 0.0 and 1.0.
		/// </summary>
		/// <returns>
		/// A pseudo-random double-precision floating-point number.
		/// </returns>
		public double NextDouble()
		{
			byte[] bytes = NextBytes(8);

			ulong value = BitConverter.ToUInt64(bytes, 0);

			return value / (double)ulong.MaxValue;
		}
	}
}
