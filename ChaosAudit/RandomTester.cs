namespace ChaosAudit
{
	/// <summary>
	/// Provides statistical validation methods for evaluating
	/// the quality and distribution characteristics of random data.
	/// </summary>
	public static class RandomTester
	{
		/// <summary>
		/// Performs a frequency (monobit) test on the supplied data.
		/// This test evaluates whether the number of 1-bits and 0-bits
		/// are approximately balanced.
		/// </summary>
		/// <param name="data">
		/// The byte array containing random data to analyze.
		/// </param>
		/// <returns>
		/// A <see cref="FrequencyTestResult"/> containing
		/// bit distribution statistics.
		/// </returns>
		public static FrequencyTestResult FrequencyTest(byte[] data)
		{
			int ones = 0;
			int totalBits = data.Length * 8;

			foreach (byte b in data)
			{
				for (int i = 0; i < 8; i++)
				{
					if (((b >> i) & 1) == 1)
						ones++;
				}
			}

			int zeros = totalBits - ones;
			double ratio = ones / (double)totalBits;

			return new FrequencyTestResult(totalBits, ones, zeros, ratio);
		}

		/// <summary>
		/// Performs a byte distribution test using the Chi-Square method.
		/// This test evaluates how evenly byte values are distributed
		/// across the entire random data set.
		/// </summary>
		/// <param name="data">
		/// The byte array containing random data to analyze.
		/// </param>
		/// <returns>
		/// A <see cref="ByteDistributionTestResult"/> containing
		/// the calculated Chi-Square statistic.
		/// </returns>
		public static ByteDistributionTestResult ByteDistributionTest(byte[] data)
		{
			int[] counts = new int[256];

			foreach (byte b in data)
			{
				counts[b]++;
			}

			double expected = data.Length / 256.0;
			double chiSquare = 0;

			for (int i = 0; i < 256; i++)
			{
				double diff = counts[i] - expected;
				chiSquare += (diff * diff) / expected;
			}

			return new ByteDistributionTestResult(chiSquare);
		}

		/// <summary>
		/// Performs a serial correlation test on the supplied data.
		/// This test measures the statistical dependency between
		/// adjacent bytes in the random sequence.
		/// </summary>
		/// <param name="data">
		/// The byte array containing random data to analyze.
		/// </param>
		/// <returns>
		/// A <see cref="SerialCorrelationTestResult"/> containing
		/// the calculated serial correlation coefficient.
		/// </returns>
		public static SerialCorrelationTestResult SerialCorrelationTest(byte[] data)
		{
			int n = data.Length;
			double mean = data.Average(b => (double)b);
			double numerator = 0;
			double denominator = 0;

			for (int i = 0; i < n - 1; i++)
			{
				numerator += (data[i] - mean) * (data[i + 1] - mean);
			}

			for (int i = 0; i < n; i++)
			{
				denominator += Math.Pow(data[i] - mean, 2);
			}

			double correlation = numerator / denominator;

			return new SerialCorrelationTestResult(correlation);
		}
	}
}
