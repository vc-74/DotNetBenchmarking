using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.Collections;

/// <summary>
/// Compares methods of slicing a collection.
/// </summary>
[MemoryDiagnoser]
public class Slicing
{
    [Params(1_000, 10_000, 100_000)]
    public int ValueCount { get; set; }

    [Params(1_000, 10_000, 100_000)]
    public int SliceSize { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Random random = new();
        _values = Enumerable.Range(0, ValueCount).Select(i => (double)random.Next()).ToList();
        _sliceCount = ValueCount / SliceSize;
    }
    private IReadOnlyList<double> _values = null!;
    private int _sliceCount;

    [Benchmark]
    public void SliceEnumerableLinq() => Execute(SliceEnumerableLinq);

    [Benchmark]
    public void SliceEnumerableManual() => Execute(SliceEnumerableManual);

    [Benchmark]
    public void SliceClearedListLinq() => Execute(SliceClearedListLinq);

    [Benchmark]
    public void SliceClearedListManual() => Execute(SliceClearedListManual);

    [Benchmark(Baseline = true)]
    public void SliceIndexesManual() => Execute(SliceIndexesManual);

    private IReadOnlyList<double> Execute(Func<IReadOnlyList<double>, IReadOnlyList<double>> implementation) => implementation(_values);

    private IReadOnlyList<double> SliceEnumerableLinq(IReadOnlyList<double> values)
    {
        List<double> result = new(_sliceCount);

        for (int i = 0; i < _sliceCount; i++)
        {
            IEnumerable<double> slice = values.Skip(i * SliceSize).Take(SliceSize);

            double average = slice.Average();
            result.Add(average);
        }

        return result;
    }

    private static double GetAverageManual(IEnumerable<double> values)
    {
        int count = 0;
        double sum = 0;

        foreach (double value in values)
        {
            sum += value;
            count++;
        }

        return sum / count;
    }

    private IReadOnlyList<double> SliceEnumerableManual(IReadOnlyList<double> values)
    {
        List<double> result = new(_sliceCount);

        for (int i = 0; i < _sliceCount; i++)
        {
            IEnumerable<double> slice = values.Skip(i * SliceSize).Take(SliceSize);

            double average = GetAverageManual(slice);
            result.Add(average);
        }

        return result;
    }

    private static double GetAverageManual(IReadOnlyList<double> values, int startIndex, int length)
    {
        int maxIndex = startIndex + length;
        double sum = 0;

        for (int j = startIndex; j < maxIndex; j++)
        {
            sum += values[j];
        }

        return sum / length;
    }

    private IReadOnlyList<double> SliceClearedListLinq(IReadOnlyList<double> values)
    {
        List<double> result = new(_sliceCount);
        List<double> slice = new(SliceSize);

        for (int i = 0; i < _sliceCount; i++)
        {
            slice.Clear();
            for (int j = 0; j < SliceSize; j++)
            {
                double value = values[(i * SliceSize) + j];
                slice.Add(value);
            }

            double average = slice.Average();
            result.Add(average);
        }

        return result;
    }

    private IReadOnlyList<double> SliceClearedListManual(IReadOnlyList<double> values)
    {
        List<double> result = new(_sliceCount);
        List<double> slice = new(SliceSize);

        for (int i = 0; i < _sliceCount; i++)
        {
            slice.Clear();
            for (int j = 0; j < SliceSize; j++)
            {
                double value = values[(i * SliceSize) + j];
                slice.Add(value);
            }

            double average = GetAverageManual(slice, 0, SliceSize);
            result.Add(average);
        }

        return result;
    }

    private IReadOnlyList<double> SliceIndexesManual(IReadOnlyList<double> values)
    {
        List<double> result = new(_sliceCount);

        for (int i = 0; i < _sliceCount; i++)
        {
            double average = GetAverageManual(values, i * SliceSize, SliceSize);
            result.Add(average);
        }

        return result;
    }
}