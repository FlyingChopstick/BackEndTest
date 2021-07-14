using BenchmarkDotNet.Running;
using Serializer.Benchmark;

namespace Serializer
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
