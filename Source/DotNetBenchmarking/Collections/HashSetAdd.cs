using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Collections;

[MemoryDiagnoser]
public class HashSetAdd
{
    [Params(10, 1_000, 10_000, 100_000)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public void UnionDoubleEnumeration()
    {
        List<int> a = Enumerable.Range(0, Count).ToList();
        List<int> b = Enumerable.Range((Count / 2), Count).ToList();

        HashSet<int> union = new();

        foreach (int x in a)
        {
            union.Add(x);
        }

        foreach (int x in b)
        {
            union.Add(x);
        }
    }

    [Benchmark]
    public void UnionDoubleEnumerationLocalFunction()
    {
        List<int> a = Enumerable.Range(0, Count).ToList();
        List<int> b = Enumerable.Range((Count / 2), Count).ToList();

        HashSet<int> union = new();

        void addCollection(IEnumerable<int> col)
        {
            foreach (int x in col)
            {
                union.Add(x);
            }
        }

        addCollection(a);
        addCollection(b);
    }

    [Benchmark]
    public void UnionConcat()
    {
        List<int> a = Enumerable.Range(0, Count).ToList();
        List<int> b = Enumerable.Range((Count / 2), Count).ToList();

        HashSet<int> union = new();

        foreach (int x in a.Concat(b))
        {
            union.Add(x);
        }
    }
}