using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Compares methods of counting the number of set bits from a bit collection using for loops.
/// </summary>
[MemoryDiagnoser]
public class CountSetBitsFor
{
    /// <summary>
    /// Number of elements in a bit collection.
    /// </summary>
    [Params(100, 1_000, 10_000)]
    public int Count;

    /// <summary>
    /// Create and initialize an array of <see cref="bool"/>
    /// and count the number of true elements.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void BooleanArray()
    {
        bool[] bits = new bool[Count];

        for (int i = 0; i < bits.Length; i++)
        {
            bits[i] = IsEven(i);
        }

        int setBitCount = 0;

        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i])
            {
                setBitCount++;
            }
        }
    }

    private static bool IsEven(int x) => ((x % 2) == 0);

    /// <summary>
    /// Create and initialize a <see cref="BitArray"/> using a loop
    /// and count the number of true elements using a loop.
    /// </summary>
    [Benchmark]
    public void BitArrayLoopInitializationLoopCount()
    {
        BitArray bitArray = new(Count);

        for (int i = 0; i < bitArray.Count; i++)
        {
            bitArray[i] = IsEven(i);
        }

        int setBitCount = 0;

        for (int i = 0; i < bitArray.Count; i++)
        {
            if (bitArray[i])
            {
                setBitCount++;
            }
        }
    }

    /// <summary>
    /// Create and initialize a <see cref="BitArray"/> from a <see cref="bool"/> array
    /// and count the number of true elements using a loop.
    /// </summary>
    [Benchmark]
    public void BitArrayArrayInitializationLoopCount()
    {
        bool[] bits = Enumerable.Range(0, Count).Select(IsEven).ToArray();
        BitArray bitArray = new(bits);

        int setBitCount = 0;

        for (int i = 0; i < bitArray.Count; i++)
        {
            if (bitArray[i])
            {
                setBitCount++;
            }
        }
    }

    /// <summary>
    /// Create and initialize a <see cref="BitArray"/> from a <see cref="bool"/> array
    /// and count the number of true elements using Linq.
    /// </summary>
    [Benchmark]
    public void BitArrayArrayInitializationLinqCount()
    {
        bool[] bits = Enumerable.Range(0, Count).Select(IsEven).ToArray();
        BitArray bitArray = new(bits);

        int setBitCount = Enumerable.Range(0, Count).Where(i => bitArray[i]).Count();
    }
}