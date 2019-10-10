using System;
using BenchmarkDotNet.Running;

namespace BenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = new BenchmarkSwitcher(typeof(Program).Assembly).Run(args);
            //var summary = BenchmarkRunner.Run<MemoryBenchmark>();

            //var s = new SerializationBenchmarks();
            //s.Setup();

            //s.SerializeToString_TextJson();
            //s.SerializeToString_Newtonsoft();
            //s.SerializeToString_DataContract();
            //s.SerializeToString_Binary();
            //s.SerializeToString_ProtoBuf();
            //s.SerializeToString_Jil();
            //s.SerializeToString_NetSerializer();


            //s.DeserializeFromString_TextJson();
            //s.DeserializeFromString_Newtonsoft();
            //s.DeserializeFromString_DataContract();
            //s.DeserializeFromString_Binary();
            //s.DeserializeFromString_ProtoBuf();
            //s.DeserializeFromString_Jil();
            //s.DeserializeFromString_NetSerializer();

            Console.Read();
        }
    }
}
