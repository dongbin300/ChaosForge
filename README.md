# ChaosForge

A high-entropy cryptographic random number generator for .NET that combines entropy pooling, SHA-256 whitening, and counter-driven output generation.

## Overview

ChaosForge provides a `SecureRandomEngine` that collects entropy from multiple system-level sources and uses advanced techniques to produce high-quality random data suitable for cryptographic and security-sensitive applications.

## Features

- **Multi-source entropy collection**: Gathers unpredictable data from timing jitter, thread scheduling, garbage collection timing, GUID generation, and OS cryptographic providers
- **SHA-256 whitening**: Ensures uniform distribution of random output
- **Periodic reseeding**: Automatically reseeds every 1024 operations for enhanced security
- **Counter-mode generation**: Uses an incrementing counter to produce unique output blocks
- **Statistical validation**: Includes test utilities for randomness quality assessment

## Installation

Clone the repository and build the solution:

```bash
git clone https://github.com/yourusername/ChaosForge.git
cd ChaosForge
dotnet build
```

## Usage

### Basic Usage

```csharp
using ChaosForge;

var rng = new SecureRandomEngine();

// Generate random bytes
byte[] randomData = rng.NextBytes(32);

// Generate random integers
int randomInt = rng.NextInt();

// Generate random double (0.0 to 1.0)
double randomDouble = rng.NextDouble();

// Manually reseed for additional entropy
rng.Reseed();
```

### Statistical Testing

The `ChaosAudit` module provides randomness quality tests:

```csharp
using ChaosAudit;

byte[] data = rng.NextBytes(1_000_000);

// Frequency test
var freqTest = RandomTester.FrequencyTest(data);
Console.WriteLine($"1s Ratio: {freqTest.Ratio}");

// Byte distribution test
var distTest = RandomTester.ByteDistributionTest(data);
Console.WriteLine($"Chi-Square: {distTest.ChiSquare}");

// Serial correlation test
var corrTest = RandomTester.SerialCorrelationTest(data);
Console.WriteLine($"Correlation: {corrTest.Correlation}");
```

## Project Structure

- **ChaosForge**: Core library containing the random engine and entropy collection
- **ChaosForge.Example**: Sample application demonstrating usage and statistical validation
- **ChaosAudit**: Testing utilities for randomness quality assessment

## Requirements

- .NET 6.0 or higher
- Supports multiple .NET versions (net6.0, net8.0, net9.0, net10.0)

## Security Considerations

While ChaosForge provides high-entropy random generation, ensure it meets your specific security requirements before using in production systems. Always validate randomness quality for your use case.

## License

[MIT License]