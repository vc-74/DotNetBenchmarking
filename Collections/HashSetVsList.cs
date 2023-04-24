using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Collections;

/// <summary>
/// Compares methods of searching for a given string using a list vs a hashset.
/// </summary>
[MemoryDiagnoser]
public class HashSetVsList
{
    [Params(5, 10, 20, 100)]
    public int KeyCount { get; set; }

    private IReadOnlyList<string> _keys = null!;
    private ISet<string> _set = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _keys = Enumerable.Range(1, KeyCount).Select(i => i.ToString()).ToList();
        _set = new HashSet<string>(_keys);
    }

    [Benchmark]
    public void ListFound()
    {
        string toSearch = (KeyCount / 2).ToString();
        bool b = _keys.Contains(toSearch);
    }

    [Benchmark]
    public void ListNotFound()
    {
        string toSearch = (KeyCount + 1).ToString();
        bool b = _keys.Contains(toSearch);
    }

    [Benchmark]
    public void SetFound()
    {
        string toSearch = (KeyCount / 2).ToString();
        bool b = _set.Contains(toSearch);
    }

    [Benchmark]
    public void SetNotFound()
    {
        string toSearch = (KeyCount + 1).ToString();
        bool b = _set.Contains(toSearch);
    }
}
