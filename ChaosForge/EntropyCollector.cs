using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ChaosForge
{
	/// <summary>
	/// Provides entropy collection utilities for gathering
	/// unpredictable system and timing-based data sources.
	/// </summary>
	public static class EntropyCollector
	{
		/// <summary>
		/// Collects entropy from multiple system-level sources
		/// including timing jitter, thread scheduling behavior,
		/// garbage collection timing, GUID generation, and the
		/// operating system cryptographic random provider.
		/// </summary>
		/// <returns>
		/// A SHA-256 hashed byte array representing the collected entropy.
		/// </returns>
		public static byte[] Collect()
		{
			var sb = new StringBuilder();

			sb.Append(DateTime.UtcNow.Ticks);
			sb.Append(Environment.TickCount64);
			sb.Append(Guid.NewGuid());

			// Stopwatch jitter
			long prev = Stopwatch.GetTimestamp();

			for (int i = 0; i < 1000; i++)
			{
				long now = Stopwatch.GetTimestamp();
				sb.Append(now - prev);
				prev = now;
			}

			// Thread race entropy
			Parallel.For(0, 128, i =>
			{
				lock (sb)
				{
					sb.Append(Stopwatch.GetTimestamp());
				}
			});

			// GC timing
			GC.Collect();
			sb.Append(Stopwatch.GetTimestamp());

			// OS secure RNG
			byte[] osRandom = new byte[64];
			RandomNumberGenerator.Fill(osRandom);

			sb.Append(Convert.ToBase64String(osRandom));

			return SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
		}
	}
}
