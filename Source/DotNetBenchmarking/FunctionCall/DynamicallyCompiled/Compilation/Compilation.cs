using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

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
    public void DynamicMethodStaticDelegate() => AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Static);

    [Benchmark]
    public void DynamicMethodInstanceDelegate() => AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Instance);

    // Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeNewModuleStaticDelegate() => AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Static);

    // Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeNewModuleInstanceDelegate() => AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Instance);

    // Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeExistingModuleStaticDelegate() => AdderTypeFactory.GetAdd(buildModule: false, DelegateInstanceType.Static);

    // Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeExistingModuleInstanceDelegate() => AdderTypeFactory.GetAdd(buildModule: false, DelegateInstanceType.Instance);

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
