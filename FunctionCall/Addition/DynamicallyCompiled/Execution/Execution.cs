using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.DynamicallyCompiled;

/// <summary>
/// Compares methods of calling dymanic functions (late binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Execution
{
    [Params(1_000, 10_000, 100_000)]
    public int Loops { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _addILStatic = AddMethodFactory.BuildDynamicMethodIL(DelegateType.Static);
        _addILInstance = AddMethodFactory.BuildDynamicMethodIL(DelegateType.Instance);

        _typeAddStatic = AdderTypeFactory.BuildTypeIL(buildModule: true, DelegateType.Static);
        _typeAddInstance = AdderTypeFactory.BuildTypeIL(buildModule: true, DelegateType.Instance);

        _addExpressionTree = AddMethodFactory.BuildDynamicMethodExpressionTree();
    }
    private TakesTwoIntsReturnsInt _addILStatic = null!, _addILInstance = null!;
    private TakesTwoIntsReturnsInt _typeAddStatic = null!, _typeAddInstance = null!;
    private TakesTwoIntsReturnsInt _addExpressionTree = null!;

    [Benchmark(Baseline = true)]
    public void DynamicMethodInstance()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            int b = 2;
            int c = _addILInstance(a, b);
        }
    }

    [Benchmark]
    public void DynamicMethodStatic()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = _addILStatic(a, b);
        }
    }

    [Benchmark]
    public void DynamicTypeMethodInstance()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = _typeAddInstance(a, b);
        }
    }

    [Benchmark]
    public void DynamicTypeMethodStatic()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = _typeAddStatic(a, b);
        }
    }

    [Benchmark]
    public void ExpressionTree()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = _addExpressionTree(a, b);
        }
    }
}
