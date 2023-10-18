using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Compares methods of initializing bit collections.
/// </summary>
[MemoryDiagnoser]
public class Initialization
{
    /// <summary>
    /// Number of elements in a bit collection.
    /// </summary>
    [Params(100, 1_000, 10_000)]
    public int Count { get; set; }

    /// <summary>
    /// Creates and initializes an array of <see cref="bool"/>.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void BooleanArray()
    {
        bool[] bits = new bool[Count];

        for (int i = 0; i < bits.Length; i++)
        {
            bits[i] = IsEven(i);
        }
    }

    private static bool IsEven(int i) => ((i % 2) == 0);

    /// <summary>
    /// Creates and initializes a <see cref="BitArray"/> using a loop.
    /// </summary>
    [Benchmark]
    public void BitArrayFor()
    {
        BitArray bitArray = new(Count);

        for (int i = 0; i < bitArray.Count; i++)
        {
            bitArray[i] = IsEven(i);
        }
    }

    /// <summary>
    /// Creates and initializes a <see cref="BitArray"/> from a <see cref="bool"/> array.
    /// </summary>
    [Benchmark]
    public void BitArrayFromArray()
    {
        bool[] bits = new bool[Count];
        for (int i = 0; i < bits.Length; i++)
        {
            bits[i] = IsEven(i);
        }

        new BitArray(bits);
    }
}