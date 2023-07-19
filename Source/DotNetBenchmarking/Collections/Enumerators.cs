using System.Collections;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Collections;

/// <summary>
/// Compares methods of enumerating elements.
/// </summary>
[MemoryDiagnoser]
public class Enumerators
{
    /// <summary>
    /// Number of elements in a bit collection.
    /// </summary>
    [Params(10_000, 100_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = Enumerable.Range(1, Count).ToArray();
        _list = _array.ToList();
        _intEnumerable = new IntEnumerable(Count);
        _iteratorMethodResult = GetInts(Count);
    }
    private int[] _array = null!;
    private List<int> _list = null!;
    private IntEnumerable _intEnumerable = null!;
    private IEnumerable<int> _iteratorMethodResult = null!;

    [Benchmark(Baseline = true)]
    public void Array()
    {
        int max = 0;
        foreach (int i in _array)
        {
            if (i > max)
            {
                max = i;
            }
        }
    }

    [Benchmark]
    public void List()
    {
        int max = 0;
        foreach (int i in _list)
        {
            if (i > max)
            {
                max = i;
            }
        }
    }

    [Benchmark]
    public void CustomEnumerable()
    {
        int max = 0;
        foreach (int i in _intEnumerable)
        {
            if (i > max)
            {
                max = i;
            }
        }
    }

    private class IntEnumerator : IEnumerator<int>
    {
        public IntEnumerator(int count)
        {
            _count = count;
        }
        private readonly int _count;

        public int Current => _current;
        private int _current = 0;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_current == _count)
            {
                return false;
            }

            _current++;
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        { }
    }

    private class IntEnumerable : IEnumerable<int>
    {
        public IntEnumerable(int count)
        {
            _count = count;
        }
        private readonly int _count;

        public IEnumerator<int> GetEnumerator() => new IntEnumerator(_count);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Benchmark]
    public void IteratorMethod()
    {
        int max = 0;
        foreach (int i in _iteratorMethodResult)
        {
            if (i > max)
            {
                max = i;
            }
        }
    }

    private static IEnumerable<int> GetInts(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            yield return i;
        }
    }
}
