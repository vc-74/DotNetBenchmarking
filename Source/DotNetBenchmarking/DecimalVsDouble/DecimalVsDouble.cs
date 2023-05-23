using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.DecimalVsDouble;

/// <summary>
/// Compares <see cref="double"/> performance vs <see cref="decimal"/> for arithmetic operations.
/// </summary>
[MemoryDiagnoser]
public class DecimalVsDouble
{
    [Params(1_000, 10_000, 100_000)]
    public int ValueCount { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Random random = new();

        List<int> baseValues = Enumerable.Range(0, ValueCount).Select(i => random.Next()).ToList();
        _doubleValues = baseValues.Select(i => (double)i).ToArray();
        _decimalValues = baseValues.Select(i => (decimal)i).ToArray();
    }
    private double[] _doubleValues = null!;
    private decimal[] _decimalValues = null!;

    [Benchmark]
    public void Double()
    {
        double average = 0;
        for (int i = 0; i < ValueCount; i++)
        {
            average = ((i * average) + _doubleValues[i]) / (i + 1);
        }
    }

    [Benchmark]
    public void Decimal()
    {
        decimal average = 0;
        for (int i = 0; i < ValueCount; i++)
        {
            average = ((i * average) + _decimalValues[i]) / (i + 1);
        }
    }
}
