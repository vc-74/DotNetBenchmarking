using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

/// <summary>
/// Compares methods of calling dymanic functions (late binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD, Column.IterationCount, Column.WarmupCount)]
[MemoryDiagnoser]
public class CompilationAndExecution
{
    [Params(10_000, 50_000, 100_000)]
    public int Loops { get; set; }

    [Benchmark(Baseline = true)]
    public void NoFunction()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = a + i;
        }
    }

    [Benchmark]
    public void StaticFunction() => StaticFunction(Loops);

    private static void StaticFunction(int loops)
    {
        int a = 1;

        for (int i = 0; i < loops; i++)
        {
            int _ = a + i;
        }
    }

    [Benchmark]
    public void DynamicMethodStaticDelegate() => Execute(AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Static));

    private void Execute(TakesTwoIntsReturnsInt add)
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            add(a, i);
        }
    }

    [Benchmark]
    public void DynamicMethodInstanceDelegate() => Execute(AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Instance));

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeStaticDelegate() => Execute(AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Static));

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeInstanceDelegate() => Execute(AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Instance));

    [Benchmark]
    public void ExpressionTreeBuilt() => Execute(AddMethodFactory.GetFromExpressionTree(useLambda: false));

    [Benchmark]
    public void ExpressionTreeFromLambda() => Execute(AddMethodFactory.GetFromExpressionTree(useLambda: true));

    [Benchmark]
    public void LoopDynamicMethodStatic()
    {
        AddLoop addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        Execute(addLoop);
    }

    private void Execute(AddLoop addLoop) => addLoop(Loops);

    [Benchmark]
    public void LoopDynamicMethodInstance()
    {
        AddLoop addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        Execute(addLoop);
    }

    [Benchmark]
    public void LoopDynamicMethodEmbedded()
    {
        AddLoop addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.Embedded);
        Execute(addLoop);
    }
}
