using BenchmarkDotNet.Running;

namespace DotNetBenchmarking;

internal class Program
{
    static void Main(string[] _)
    {
        BenchmarkRunner.Run(typeof(BitCollection.ReadWriteBits));
        /*BenchmarkRunner.Run(typeof(BitCollection.CountSetBits));
        BenchmarkRunner.Run(typeof(BitCollection.ReadWriteBits));*/
    }
}
