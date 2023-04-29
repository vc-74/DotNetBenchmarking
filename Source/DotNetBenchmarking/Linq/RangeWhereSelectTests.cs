using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Linq;

/// <summary>
/// Compares methods of filtering a collection.
/// </summary>
[MemoryDiagnoser]
public class RangeWhereSelectTests
{
    [Params(100, 1_000, 10_000)]
    public int Count;

    [Benchmark(Baseline = true)]
    public void Manual()
    {
        int[] indexes = new int[Count];

        int j = 0;
        for (int i = 0; i < Count; i++)
        {
            if ((i % 2) == 0)
            {
                indexes[j++] = i;
            }
        }
    }

    [Benchmark]
    public void EnumerableMethod()
    {
        int[] _ = GetIndexes().ToArray();
    }

    private IEnumerable<int> GetIndexes()
    {
        for (int i = 0; i < Count; i++)
        {
            if ((i % 2) == 0)
            {
                yield return i;
            }
        }
    }

    [Benchmark]
    public void Linq()
    {
        int[] indexes = Enumerable.Range(0, Count).Where(i => ((i % 2) == 0)).Select(index => index + 1).ToArray();
    }
}