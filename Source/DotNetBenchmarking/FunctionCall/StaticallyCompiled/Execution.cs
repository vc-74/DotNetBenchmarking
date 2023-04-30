using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.StaticallyCompiled;

/// <summary>
/// Compares methods of calling non dymanic functions (early binding) 
/// executing a loop adding two integers.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Execution
{
    [Params(1_000)]
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
    public void StaticMethod()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = AddStatic(a, i);
        }
    }

    private static int AddStatic(int a, int b) => a + b;

    /// <summary>
    /// Implementation as an instance method call.
    /// </summary>
    [Benchmark]
    public void InstanceMethod()
    {
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = AddInstance(i);
        }
    }

    private int AddInstance(int b) => _a + b;

#pragma warning disable IDE0044 // Add readonly modifier
    private int _a = 1;
#pragma warning restore IDE0044 // Add readonly modifier

    /// <summary>
    /// Implementation as a virtual method call.
    /// </summary>
    [Benchmark]
    public void VirtualMethod()
    {
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = AddVirtual(i);
        }
    }

    protected virtual int AddVirtual(int b) => _a + b;

    /// <summary>
    /// Implementation as a static local function call.
    /// </summary>
    [Benchmark]
    public void StaticLocalFunction()
    {
        static int addLocal(int a, int b) => a + b;

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = addLocal(a, i);
        }
    }

    /// <summary>
    /// Implementation as an instance local function call.
    /// </summary>
    [Benchmark]
    public void InstanceLocalFunction()
    {
        int addLocal(int b) => _a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = addLocal(i);
        }
    }

    /// <summary>
    /// Implementation as an instance local function capturing a local variable call.
    /// </summary>
    [Benchmark]
    public void InstanceLocalFunctionCapture()
    {
        int a = 1;
        int addLocal(int b) => a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = addLocal(i);
        }
    }

    /// <summary>
    /// Implementation as a func call from a lambda expression.
    /// </summary>
    [Benchmark]
    public void Lambda()
    {
#pragma warning disable IDE0039 // Use local function
        Func<int, int, int> add = (a, b) => a + b;
#pragma warning restore IDE0039 // Use local function

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = add(a, i);
        }
    }

    /// <summary>
    /// Implementation as a func call from a lambda expression capturing a local variable.
    /// </summary>
    [Benchmark]
    public void LambdaCapture()
    {
        int a = 1;
#pragma warning disable IDE0039 // Use local function
        Func<int, int> add = b => a + b;
#pragma warning restore IDE0039 // Use local function

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = add(i);
        }
    }

    /// <summary>
    /// Implementation as an invocation of a delegate instance on a static method.
    /// </summary>
    [Benchmark]
    public void DelegateStaticMethod()
    {
        TakesTwoIntsReturnsInt add = AddStatic;

        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = add(a, i);
        }
    }

    /// <summary>
    /// Implementation as an invocation of a delegate instance on an instance method.
    /// </summary>
    [Benchmark]
    public void DelegateInstanceMethod()
    {
        TakesOneIntReturnsInt add = AddInstance;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = add(i);
        }
    }

    /// <summary>
    /// Implementation as an invocation of a delegate instance on a virtual method.
    /// </summary>
    [Benchmark]
    public void DelegateVirtualMethod()
    {
        TakesOneIntReturnsInt add = AddVirtual;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = add(i);
        }
    }
}
