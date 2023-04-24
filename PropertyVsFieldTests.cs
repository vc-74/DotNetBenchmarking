using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares property get access to field access.
/// </summary>
[MemoryDiagnoser]
public class PropertyVsFieldTests
{
    [Benchmark(Baseline = true)]
    public void Property()
    {
        string s = TestClass.Instance.Property;
        string s2 = s + s;
    }

    private class TestClass
    {
        public static TestClass Instance { get; } = new();

        public string Property => Field;

        public string Field = "Blah";
    }

    [Benchmark]
    public void Field()
    {
        string s = TestClass.Instance.Field;
        string s2 = s + s;
    }
}