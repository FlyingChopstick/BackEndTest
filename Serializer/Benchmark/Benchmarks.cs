using System.IO;
using BenchmarkDotNet.Attributes;
using Serializer.Test;

namespace Serializer.Benchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class Benchmarks
    {
        DLList list = new();
        ListSerializer serializer = new();
        string filename = "dump.txt";


        public Benchmarks()
        {
            for (int i = 0; i < 10; i++)
            {
                list.Add($"Item #{i}");
            }

            list.SetReference(3, 1);
            list.SetReference(5, 7);
            list.SetReference(4, 2);
        }

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
