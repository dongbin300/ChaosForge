namespace ChaosAudit
{
	/// <summary>
	/// Represents the result of a frequency (monobit) test.
	/// This test measures the balance between 1-bits and 0-bits
	/// in the generated random data stream.
	/// </summary>
	/// <param name="TotalBits">
	/// The total number of bits analyzed during the test.
	/// </param>
	/// <param name="Ones">
	/// The total number of bits with value 1.
	/// </param>
	/// <param name="Zeros">
	/// The total number of bits with value 0.
	/// </param>
	/// <param name="Ratio">
	/// The ratio of 1-bits to the total number of bits.
	/// An ideal random source should produce a value close to 0.5.
	/// </param>
	public record FrequencyTestResult(int TotalBits, int Ones, int Zeros, double Ratio);

	/// <summary>
	/// Represents the result of a byte distribution test
	/// using the Chi-Square statistical method.
	/// This test evaluates how uniformly byte values
	/// are distributed across the random data set.
	/// </summary>
	/// <param name="ChiSquare">
	/// The calculated Chi-Square value.
	/// Lower values generally indicate a more uniform distribution.
	/// </param>
	public record ByteDistributionTestResult(double ChiSquare);

	/// <summary>
	/// Represents the result of a serial correlation test.
	/// This test measures the statistical dependency
	/// between adjacent bytes in the random sequence.
	/// </summary>
	/// <param name="Correlation">
	/// The calculated serial correlation coefficient.
	/// Values close to 0 indicate low correlation
	/// and better randomness quality.
	/// </param>
	public record SerialCorrelationTestResult(double Correlation);
}