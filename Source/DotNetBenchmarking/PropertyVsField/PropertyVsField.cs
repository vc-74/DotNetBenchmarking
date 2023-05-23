using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares property get access to field access.
/// </summary>
[MemoryDiagnoser]
public class PropertyVsField
{
    [Benchmark(Baseline = true)]
    public void Property()
    {
        string s = TestClass.Instance.Property;
        string _ = s + s;
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
        string _ = s + s;
    }
}