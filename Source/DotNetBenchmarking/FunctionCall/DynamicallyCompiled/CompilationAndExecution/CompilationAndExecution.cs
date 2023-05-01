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

    /// <summary>
    /// Implementation not using functions.
    /// </summary>
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

    /// <summary>
    /// Implementation as a static method call.
    /// </summary>
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

    /// <summary>
    /// Implementation as an external loop invoking a static delegate built from a DynamicMethod.
    /// </summary>
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

    /// <summary>
    /// Implementation as an external loop invoking an instance delegate built from a DynamicMethod.
    /// </summary>
    [Benchmark]
    public void DynamicMethodInstanceDelegate() => Execute(AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Instance));

    /// <summary>
    /// Implementation as an external loop invoking a static delegate built from a dynamically built type.
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeStaticDelegate() => Execute(AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Static));

    /// <summary>
    /// Implementation as an external loop invoking an instance delegate built from a dynamically built type.
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeInstanceDelegate() => Execute(AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Instance));

    /// <summary>
    /// Implementation as an external loop invoking a delegate manually built from an expression tree.
    /// </summary>
    [Benchmark]
    public void ExpressionTreeBuilt() => Execute(AddMethodFactory.GetFromExpressionTree(useLambda: false));

    /// <summary>
    /// Implementation as an external loop invoking a delegate built from an expression tree generated from a lambda expression.
    /// </summary>
    [Benchmark]
    public void ExpressionTreeFromLambda() => Execute(AddMethodFactory.GetFromExpressionTree(useLambda: true));

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from a DynamicMethod invoking a static add delegate.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodStatic()
    {
        TakesAnInt addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.StaticDelegate);
        Execute(addLoop);
    }

    private void Execute(TakesAnInt addLoop) => addLoop(Loops);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from a DynamicMethod invoking an instance add delegate.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodInstance()
    {
        TakesAnInt addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.InstanceDelegate);
        Execute(addLoop);
    }

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from a DynamicMethod invoking embedding the addition.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodEmbedded()
    {
        TakesAnInt addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.Embedded);
        Execute(addLoop);
    }

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from an expression tree invoking a static add delegate.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeStatic()
    {
        TakesAnInt addLoop = AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.StaticDelegate);
        Execute(addLoop);
    }

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from an expression tree invoking an instance add delegate.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeInstance()
    {
        TakesAnInt addLoop = AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.InstanceDelegate);
        Execute(addLoop);
    }

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from an expression tree invoking embedding the addition.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeEmbedded()
    {
        TakesAnInt addLoop = AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.Embedded);
        Execute(addLoop);
    }
}
