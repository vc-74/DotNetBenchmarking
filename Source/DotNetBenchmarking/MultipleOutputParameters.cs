#if NET7_0
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares methods of returning multiple output parameters.
/// </summary>
[MemoryDiagnoser]
public partial class MultipleOutputParameters
{
    [Benchmark(Baseline = true)]
    public void OutParameters()
    {
        for (int i = 0; i < 1_000; i++)
        {
            int x = 10, y = 3;
            int quotient = Math.DivRem(x, y, out int remainder);
        }
    }

    [Benchmark]
    public void ValueTuple()
    {
        for (int i = 0; i < 1_000; i++)
        {
            int x = 10, y = 3;
            (int quotient, int remainder) = Math.DivRem(x, y);
        }
    }
}
#endif
