using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Compares methods of counting the number of set bits from a bit collection using foreach loops.
/// </summary>
[MemoryDiagnoser]
public class CountSetBitsForEach
{
    /// <summary>
    /// Number of elements in a bit collection.
    /// </summary>
    [Params(100, 1_000, 10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Use an indicates enumerable to avoid compiler/JIT bound checks optimization
        _indices = Enumerable.Range(0, Count);
    }
    private IEnumerable<int> _indices = null!;

    /// <summary>
    /// Create and initialize an array of <see cref="bool"/>
    /// and count the number of true elements.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void BooleanArray()
    {
        bool[] bits = new bool[Count];

        foreach (int i in _indices)
        {
            bits[i] = IsEven(i);
        }

        int setBitCount = 0;

        foreach (int i in _indices)
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

        foreach (int i in _indices)
        {
            bitArray[i] = IsEven(i);
        }

        int setBitCount = 0;

        foreach (int i in _indices)
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
        bool[] bits = _indices.Select(IsEven).ToArray();
        BitArray bitArray = new(bits);

        int setBitCount = 0;

        foreach (int i in _indices)
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
        bool[] bits = _indices.Select(IsEven).ToArray();
        BitArray bitArray = new(bits);

        int setBitCount = _indices.Where(i => bitArray[i]).Count();
    }
}