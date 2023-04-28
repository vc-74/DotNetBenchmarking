using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.DynamicallyCompiled;

/// <summary>
/// Compares methods of compiling dymanic functions.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD, Column.IterationCount, Column.WarmupCount)]
[MemoryDiagnoser]
public class Compilation
{
    [Benchmark(Baseline = true)]
    public void DynamicMethodStaticDelegate() => AddMethodFactory.GetFromDynamicMethod(DelegateType.Static);

    [Benchmark]
    public void DynamicMethodInstanceDelegate() => AddMethodFactory.GetFromDynamicMethod(DelegateType.Instance);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeNewModule() => AdderTypeFactory.GetAdd(buildModule: true, DelegateType.Instance);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeExistingModule() => AdderTypeFactory.GetAdd(buildModule: false, DelegateType.Instance);

    [Benchmark]
    public void ExpressionTreeBuilt() => AddMethodFactory.GetFromExpressionTree(useLambda: false);

    [Benchmark]
    public void ExpressionTreeFromLambda() => AddMethodFactory.GetFromExpressionTree(useLambda: true);

    [Benchmark]
    public void LoopDynamicMethodStatic() => AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.StaticDelegate);

    [Benchmark]
    public void LoopDynamicMethodInstance() => AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.InstanceDelegate);

    [Benchmark]
    public void LoopDynamicMethodEmbedded() => AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.Embedded);
}
