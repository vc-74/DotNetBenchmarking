using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Compares methods of setting bits to true and false in a bit collection.
/// </summary>
[MemoryDiagnoser]
public class ReadWriteBits
{
    [Params(100, 1_000, 10_000)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public void BooleanArray()
    {
        Span<bool> span = new bool[Count];
        Test(span);
    }

    private static void Test(Span<bool> span)
    {
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = IsEven(i);
        }

        bool result = false;

        for (int i = 0; i < span.Length; i++)
        {
            result &= span[i];
        }
    }

    private static bool IsEven(int i) => ((i % 2) == 0);

    [Benchmark]
    public void BooleanArrayStackalloc()
    {
        Span<bool> span = stackalloc bool[Count];
        Test(span);
    }

    [Benchmark]
    public void List()
    {
        List<bool> list = new(Count);

        for (int i = 0; i < Count; i++)
        {
            bool bit = IsEven(i);
            list.Add(bit);
        }

        bool result = false;

        for (int i = 0; i < Count; i++)
        {
            result &= list[i];
        }
    }

    [Benchmark]
    public void BitArray()
    {
        BitArray bitArray = new(Count);

        for (int i = 0; i < Count; i++)
        {
            bitArray[i] = IsEven(i);
        }

        bool result = false;

        for (int i = 0; i < Count; i++)
        {
            result &= bitArray[i];
        }
    }
}