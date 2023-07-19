using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Collections;

[/// <summary>
/// Compares methods of enumerating different <see cref="IReadOnlyList{T}"/> implementations.
/// </summary>
MemoryDiagnoser]
public partial class IReadOnlyListEnumeration
{
    [Params(100, 1_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = Enumerable.Range(0, Count).ToArray();
        _list = Enumerable.Range(0, Count).ToList();
    }
    private int[] _array = null!;
    private List<int> _list = null!;

    [Benchmark(Baseline = true)]
    public void ArrayFor()
    {
        int sum = 0;
        for (int i = 0; i < _array.Length; i++)
        {
            unchecked
            {
                sum += _array[i];
            }
        }
    }

    [Benchmark]
    public void ArrayForExplicitBoundaries()
    {
        int start = 0;
        int end = _array.Length;

        int sum = 0;
        for (int i = start; i < end; i++)
        {
            unchecked
            {
                sum += _array[i];
            }
        }
    }

    [Benchmark]
    public void ArrayForCalculatedBoundaries()
    {
        // Compute start/end to avoid compiler/jit optimizations
        int getStart() => 0;
        int getEnd() => _array.Length;

        int sum = 0;
        for (int i = getStart(); i < getEnd(); i++)
        {
            unchecked
            {
                sum += _array[i];
            }
        }
    }

    [Benchmark]
    public void ArrayForEach()
    {
        int sum = 0;
        foreach (int item in _array)
        {
            unchecked
            {
                sum += item;
            }
        }
    }

    [Benchmark]
    public void ListFor()
    {
        int sum = 0;
        for (int i = 0; i < _list.Count; i++)
        {
            unchecked
            {
                sum += _list[i];
            }
        }
    }

    [Benchmark]
    public void ListForExplicitBoundaries()
    {
        int start = 0;
        int end = _list.Count;

        int sum = 0;
        for (int i = start; i < end; i++)
        {
            unchecked
            {
                sum += _list[i];
            }
        }
    }

    [Benchmark]
    public void ListForCalculatedBoundaries()
    {
        // Compute start/end to avoid compiler/jit optimizations
        int getStart() => 0;
        int getEnd() => _list.Count;

        int sum = 0;
        for (int i = getStart(); i < getEnd(); i++)
        {
            unchecked
            {
                sum += _list[i];
            }
        }
    }

    [Benchmark]
    public void ListForEach()
    {
        int sum = 0;
        foreach (int item in _list)
        {
            unchecked
            {
                sum += item;
            }
        }
    }

    [Benchmark]
    public void IReadOnlyListFor()
    {
        IReadOnlyList<int> readOnlyList = _array;

        int sum = 0;
        for (int i = 0; i < readOnlyList.Count; i++)
        {
            unchecked
            {
                sum += readOnlyList[i];
            }
        }
    }

    [Benchmark]
    public void IReadOnlyListForExplicitBoundaries()
    {
        IReadOnlyList<int> readOnlyList = _array;

        int start = 0;
        int end = readOnlyList.Count;

        int sum = 0;
        for (int i = start; i < end; i++)
        {
            unchecked
            {
                sum += readOnlyList[i];
            }
        }
    }

    [Benchmark]
    public void IReadOnlyListForCalculatedBoundaries()
    {
        IReadOnlyList<int> readOnlyList = _array;

        // Compute start/end to avoid compiler optimizations
        int getStart() => 0;
        int getEnd() => readOnlyList.Count;

        int sum = 0;
        for (int i = getStart(); i < getEnd(); i++)
        {
            unchecked
            {
                sum += readOnlyList[i];
            }
        }
    }

    [Benchmark]
    public void IReadOnlyListForEach()
    {
        IReadOnlyList<int> readOnlyList = _array;

        int sum = 0;
        foreach (int item in readOnlyList)
        {
            unchecked
            {
                sum += item;
            }
        }
    }
}