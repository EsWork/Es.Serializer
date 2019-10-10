using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace BenchmarkTest
{
    [Serializable]
    [DataContract, ProtoContract]
    public class TestClass : INetMessage
    {
        [DataMember, ProtoMember(1)]
        public short int16 { get; set; }

        [DataMember, ProtoMember(2)]
        public ushort uint16 { get; set; }

        [DataMember, ProtoMember(3)]
        public int int32 { get; set; }

        [DataMember, ProtoMember(4)]
        public int? int32N { get; set; }

        [DataMember, ProtoMember(5)]
        public uint uint32 { get; set; }

        [DataMember, ProtoMember(6)]
        public long int64 { get; set; }

        [DataMember, ProtoMember(7)]
        public ulong uint64 { get; set; }

        [DataMember, ProtoMember(8)]
        public bool boolean { get; set; }

        [DataMember, ProtoMember(9)]
        public decimal decimalV { get; set; }

        [DataMember, ProtoMember(10)]
        public double doubleV { get; set; }

        [DataMember, ProtoMember(11)]
        public float floatV { get; set; }

        [DataMember, ProtoMember(12)]
        public byte byteV { get; set; }

        [DataMember, ProtoMember(13)]
        public sbyte sbyteV { get; set; }

        [DataMember, ProtoMember(14)]
        public char charV { get; set; }

        [DataMember, ProtoMember(15)]
        public DateTime date { get; set; }

        [DataMember, ProtoMember(16)]
        public string[] arr { get; set; }

        [DataMember, ProtoMember(17)]
        public TestEnum TestEnum { get; set; }
    }

    public enum TestEnum : byte
    {
        Pending = 0,
        Approved = 1
    }
}