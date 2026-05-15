using System.Security.Cryptography;

namespace ChaosForge
{
	/// <summary>
	/// Represents a thread-safe entropy pool used to accumulate
	/// and continuously mix entropy from multiple sources.
	/// </summary>
	public class EntropyPool
	{
		/// <summary>
		/// Internal entropy state buffer.
		/// </summary>
		private byte[] _pool = new byte[32];
		/// <summary>
		/// Synchronization object used to ensure thread-safe access
		/// to the entropy pool state.
		/// </summary>
		private readonly object _lock = new();

		/// <summary>
		/// Mixes additional entropy into the internal entropy pool
		/// using SHA-256 hashing to diffuse and normalize the input.
		/// </summary>
		/// <param name="entropy">
		/// The entropy data to incorporate into the pool.
		/// </param>
		public void Mix(byte[] entropy)
		{
			lock (_lock)
			{
				byte[] combined = new byte[_pool.Length + entropy.Length];

				Buffer.BlockCopy(_pool, 0, combined, 0, _pool.Length);
				Buffer.BlockCopy(entropy, 0, combined, _pool.Length, entropy.Length);

				_pool = SHA256.HashData(combined);
			}
		}

		/// <summary>
		/// Returns a snapshot copy of the current entropy pool state.
		/// </summary>
		/// <returns>
		/// A cloned byte array representing the current entropy pool.
		/// </returns>
		public byte[] Snapshot()
		{
			lock (_lock)
			{
				return [.. _pool];
			}
		}
	}
}
