using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares methods of getting a value or a default value if not available.
/// </summary>
public class GetValueOrDefault
{
    [MemoryDiagnoser]
    public class GetValueOrDefaultTests
    {
        private const int _valuesCount = 1_000_000;

        public GetValueOrDefaultTests()
        {
            _values = Enumerable.Range(1, _valuesCount).Select(i => ((i % 2) == 0) ? i : (double?)null).ToArray();
        }
        private readonly IReadOnlyList<double?> _values;

        [Benchmark(Baseline = true)]
        public void GetValueOrDefault()
        {
            double? result = null;

            for (int i = 0; i < _values.Count; i++)
            {
                double? rawValue = _values[i];
                if (rawValue is not null)
                {
                    result = result.GetValueOrDefault(0d) + rawValue.Value;
                }
            }
        }

        [Benchmark]
        public void NullCoalescingOperator()
        {
            double? result = null;

            for (int i = 0; i < _values.Count; i++)
            {
                double? rawValue = _values[i];
                if (rawValue is not null)
                {
                    result = (result ?? 0d) + rawValue.Value;
                }
            }
        }

        [Benchmark]
        public void If()
        {
            double? result = null;

            for (int i = 0; i < _values.Count; i++)
            {
                double? rawValue = _values[i];
                if (rawValue is not null)
                {
                    if (result is null)
                    {
                        result = rawValue;
                    }
                    else
                    {
                        result = result.Value + rawValue.Value;
                    }
                }
            }
        }
    }
}
