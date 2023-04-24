using BenchmarkDotNet.Running;

namespace DotNetBenchmarking;

internal class Program
{
    static void Main(string[] _)
    {
        BenchmarkRunner.Run(typeof(FunctionCall.Addition.DynamicallyCompiled.Execution));
    }
}
