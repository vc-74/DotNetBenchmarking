using BenchmarkDotNet.Running;

namespace DotNetBenchmarking;

internal class Program
{
    static void Main(string[] _)
    {
#if NET7_0
        BenchmarkRunner.Run(typeof(Regex.Regex70));
#endif
    }
}
