using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Collections;

/// <summary>
/// Compares recreating an array vs clearing it.
/// </summary>
[MemoryDiagnoser]
public partial class ArrayClearVsRecreate
{
    [Params(100)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public void Recreate()
    {
        for (int i = 0; i < 1_000; i++)
        {
            int[] _ = new int[Count];
        }
    }

    [Benchmark]
    public void Clear()
    {
        int[] array = new int[Count];

        for (int i = 0; i < 1_000; i++)
        {
            for (int j = 0; j < array.Length; j++)
            {
                array[j] = 0;
            }
        }
    }

    [Benchmark]
    public void ArrayClear()
    {
        int[] array = new int[Count];

        for (int i = 0; i < 1_000; i++)
        {
            Array.Clear(array, 0, array.Length);
        }
    }
}