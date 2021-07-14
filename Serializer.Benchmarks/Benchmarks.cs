using System.IO;
using BenchmarkDotNet.Attributes;

namespace Serializer.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    public class Benchmarks
    {
        DLList list = DLList.NewFilledList(1000);
        ListSerializer serializer = new();
        string filename = "dump.txt";



        [Benchmark]
        public void Serialize()
        {
            using Stream writeStream = File.OpenWrite(filename);
            serializer.Serialize(list.Head, writeStream);
        }

        [Benchmark]
        public void Deserialize()
        {
            using Stream readStream = File.OpenRead(filename);
            serializer.Deserialize(readStream);
        }
    }
}
