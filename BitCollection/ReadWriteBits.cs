using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Compares methods of setting bits to true and false in a bit collection.
/// </summary>
[MemoryDiagnoser]
public class ReadWriteBits
{
    [Params(1_000)]
    public int Count;

    [Benchmark(Baseline = true)]
    public void BooleanArray()
    {
        Span<bool> span = new bool[Count];
        Test(span);
    }

    private void Test(Span<bool> span)
    {
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = ((i % 2) == 0);
        }

        bool result = false;

        for (int i = 0; i < (span.Length / 2); i++)
        {
            result &= span[i];
        }
    }

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
            bool bit = ((i % 2) == 0);
            list.Add(bit);
        }

        bool result = false;

        for (int i = 0; i < (Count / 2); i++)
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
            bitArray[i] = ((i % 2) == 0);
        }

        bool result = false;

        for (int i = 0; i < (Count / 2); i++)
        {
            result &= bitArray[i];
        }
    }

    [Benchmark]
    public void BitArrayInitializer()
    {
        BitArray bitArray = new(Enumerable.Range(0, Count).Select(i => ((i % 2) == 0)).ToArray());

        bool result = false;

        for (int i = 0; i < (Count / 2); i++)
        {
            result &= bitArray[i];
        }
    }
}