using BenchmarkDotNet.Running;

namespace DotNetBenchmarking;

internal class Program
{
    static void Main(string[] _)
    {
        /*AddLoop addLoop = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.Embedded, outputResultToConsole: true);
        addLoop(1, 1, 10);*/
        BenchmarkRunner.Run(typeof(FunctionCall.Addition.DynamicallyCompiled.CompilationAndExecution));
    }
}
