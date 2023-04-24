using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Compares methods of calling dymanic looping functions (late binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class DynamicLoopExecution
{
    [Params(1_000, 10_000, 100_000)]
    public int Loops { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _addIntegersStatic = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        _addIntegersInstance = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        _addIntegersEmbedded = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.Embedded);
    }
    private AddIntegers _addIntegersStatic = null!, _addIntegersInstance = null!, _addIntegersEmbedded = null!;

    [Benchmark(Baseline = true)]
    public void StaticDelegate()
    {
        AddIntegers addIntegers = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        addIntegers(Loops);
    }

    [Benchmark]
    public void InstanceDelegate()
    {
        AddIntegers addIntegers = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        addIntegers(Loops);
    }

    [Benchmark]
    public void Embedded()
    {
        AddIntegers addIntegers = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.Embedded);
        addIntegers(Loops);
    }

    [Benchmark]
    public void PreCompiledStaticDelegate()
    {
        _addIntegersStatic(Loops);
    }

    [Benchmark]
    public void PreCompiledInstanceDelegate()
    {
        _addIntegersInstance(Loops);
    }

    [Benchmark]
    public void PreCompiledEmbedded()
    {
        _addIntegersEmbedded(Loops);
    }
}
