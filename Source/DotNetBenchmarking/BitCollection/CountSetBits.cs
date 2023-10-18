using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Compares methods of counting the number of set bits from a bit collection.
/// </summary>
[MemoryDiagnoser]
public class CountSetBits
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = Enumerable.Range(1, Count).Select(IsEven).ToArray();
        _bitArray = new BitArray(_array);
    }
    private bool[] _array = null!;
    private BitArray _bitArray = null!;

    private static bool IsEven(int i) => ((i % 2) == 0);

    /// <summary>
    /// Number of elements in a bit collection.
    /// </summary>
    [Params(100, 1_000, 10_000)]
    public int Count { get; set; }

    /// <summary>
    /// Counts the number of true elements in a <see cref="bool[]"/>
    /// using a for loop.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void BooleanArrayFor()
    {
        int setBitCount = 0;

        // Count set bits
        for (int i = 0; i < _array.Length; i++)
        {
            if (_array[i])
            {
                setBitCount++;
            }
        }
    }

    /// <summary>
    /// Counts the number of true elements in a <see cref="bool[]"/>
    /// using a foreach loop.
    /// </summary>
    [Benchmark]
    public void BooleanArrayForEach()
    {
        int setBitCount = 0;

        // Count set bits
        foreach (bool bit in _array)
        {
            if (bit)
            {
                setBitCount++;
            }
        }
    }

    /// <summary>
    /// Counts the number of true elements in a <see cref="bool[]"/>
    /// using Linq.
    /// </summary>
    [Benchmark]
    public void BooleanArrayLinq()
    {
        int setBitCount = _array.Count(b => b);
    }

    /// <summary>
    /// Counts the number of true elements in a <see cref="BitArray"/>
    /// using a for loop.
    /// </summary>
    [Benchmark]
    public void BitArrayFor()
    {
        int setBitCount = 0;

        for (int i = 0; i < _bitArray.Count; i++)
        {
            if (_bitArray[i])
            {
                setBitCount++;
            }
        }
    }

    /// <summary>
    /// Counts the number of true elements in a <see cref="BitArray"/>
    /// using a foreach loop.
    /// </summary>
    [Benchmark]
    public void BitArrayForEach()
    {
        int setBitCount = 0;

        foreach (bool bit in _bitArray)
        {
            if (bit)
            {
                setBitCount++;
            }
        }
    }

    /// <summary>
    /// Counts the number of true elements in a <see cref="BitArray"/>
    /// using Linq.
    /// </summary>
    [Benchmark]
    public void BitArrayLinq()
    {
        int setBitCount = _bitArray.Cast<bool>().Count(b => b);
    }
}