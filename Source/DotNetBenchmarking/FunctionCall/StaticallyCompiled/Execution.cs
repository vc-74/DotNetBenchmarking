using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.StaticallyCompiled;

/// <summary>
/// Compares methods of calling non dymanic functions (early binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Execution
{
    [Params(1_000)]
    public int Loops { get; set; }

    [Benchmark(Baseline = true)]
    public void NoFunction()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = a + i;
        }
    }

    [Benchmark]
    public void StaticMethod()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            AddStatic(a, i);
        }
    }

    private static int AddStatic(int a, int b) => a + b;

    [Benchmark]
    public void InstanceMethod()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            AddInstance(a, i);
        }
    }

    private int AddInstance(int a, int b) => a + b;

    [Benchmark]
    public void VirtualMethod()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            AddInstanceVirtual(a, i);
        }
    }

    protected virtual int AddInstanceVirtual(int a, int b) => a + b;

    [Benchmark]
    public void StaticLocalFunction()
    {
        static int LocalAddStatic(int a, int b) => a + b;

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LocalAddStatic(a, i);
        }
    }

    [Benchmark]
    public void InstanceLocalFunction()
    {
        int LocalAddInstance(int a, int b) => a + b;

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LocalAddInstance(a, i);
        }
    }

    [Benchmark]
    public void InstanceLocalFunctionCapture()
    {
        int a = 1;
        int LocalAdd(int b) => a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LocalAdd(i);
        }
    }

    [Benchmark]
    public void Lambda()
    {
        Func<int, int, int> LambdaAdd = (a, b) => a + b;

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LambdaAdd(a, i);
        }
    }

    [Benchmark]
    public void LambdaCapture()
    {
        int a = 1;
        Func<int, int> LambdaAdd = b => a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LambdaAdd(i);
        }
    }

    [Benchmark]
    public void DelegateStaticMethod()
    {
        TakesTwoIntsReturnsInt StaticDelegateAdd = AddStatic;

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = StaticDelegateAdd(a, i);
        }
    }

    [Benchmark]
    public void DelegateInstanceMethod()
    {
        TakesOneIntReturnsInt InstanceDelegateAdd = AddInstance;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = InstanceDelegateAdd(i);
        }
    }

    private int _a = 1;
    private int AddInstance(int b) => _a + b;

    /// <summary>
    /// Prototype for functions taking one integer in parameter and returning an integer.
    /// </summary>
    public delegate int TakesOneIntReturnsInt(int b);

    [Benchmark]
    public void DelegateVirtualMethod()
    {
        TakesOneIntReturnsInt InstanceDelegateAdd = AddInstanceVirtual;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = InstanceDelegateAdd(i);
        }
    }

    protected virtual int AddInstanceVirtual(int b) => _a + b;
}
