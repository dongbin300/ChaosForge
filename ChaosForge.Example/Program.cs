using ChaosAudit;

using ChaosForge;

Console.WriteLine("Initializing Secure Random Engine...");
var rng = new SecureRandomEngine();
Console.WriteLine("Generating random data...");
byte[] randomData = rng.NextBytes(1_000_000);
Console.WriteLine("Done.");
Console.WriteLine();

Console.WriteLine("Sample Random Integers:");
for (int i = 0; i < 10; i++)
{
	Console.WriteLine(rng.NextInt());
}
Console.WriteLine();

// Statistical tests
var frequencyTestResult = RandomTester.FrequencyTest(randomData);
Console.WriteLine("=== Frequency Test ===");
Console.WriteLine($"Total Bits : {frequencyTestResult.TotalBits}");
Console.WriteLine($"1s         : {frequencyTestResult.Ones}");
Console.WriteLine($"0s         : {frequencyTestResult.Zeros}");
Console.WriteLine($"1 Ratio    : {frequencyTestResult.Ratio:F6}");
Console.WriteLine();

var byteDistributionTestResult = RandomTester.ByteDistributionTest(randomData);
Console.WriteLine("=== Byte Distribution Test ===");
Console.WriteLine($"Chi-Square : {byteDistributionTestResult.ChiSquare:F4}");
Console.WriteLine();

var serialCorrelationTestResult = RandomTester.SerialCorrelationTest(randomData);
Console.WriteLine("=== Serial Correlation Test ===");
Console.WriteLine($"Correlation : {serialCorrelationTestResult.Correlation:F8}");
Console.WriteLine();

Console.WriteLine("Testing complete.");