using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace PerformanceTest
{
#if NETFULL || NETCOREAPP3_0
    [Serializable]
#endif
    [DataContract, ProtoContract]
    public class Foo : INetMessage
    {
        [DataMember, ProtoMember(1)]
        public int Id { get; set; }
        [DataMember, ProtoMember(2)]
        public string Name { get; set; }
        [DataMember, ProtoMember(3)]
        public int Age { get; set; }
        [DataMember, ProtoMember(4)]
        public DateTime Date { get; set; }
        [DataMember, ProtoMember(5)]
        public bool Sex { get; set; }
        [DataMember, ProtoMember(6)]
        public Guid Xuid { get; set; }
        [DataMember, ProtoMember(7)]
        public byte[] Lastip { get; set; }
        [DataMember, ProtoMember(8)]
        public UserStatus UserStatus { get; set; }
        [DataMember, ProtoMember(9)]
        public InnerFoo Inner { get; set; }

        public override string ToString() {
            return string.Format("Id:{0} \nName:{1} \nAge:{2} \nDate:{3} \nSex:{4} \nGuid:{5} \nIP:{6} \nStatus:{7} \nInner:{8}",
                Id, Name, Age, Date, Sex, Xuid, string.Join(".", Lastip), UserStatus, Inner);
        }
    }

#if NETFULL || NETCOREAPP3_0
    [Serializable]
#endif
    [DataContract, ProtoContract]
    public class InnerFoo
    {
        [DataMember, ProtoMember(1)]
        public decimal InnerNumeric
        {
            get; set;
        }

        public override string ToString() {
            return InnerNumeric.ToString();
        }
    }

    public enum UserStatus : byte
    {
        Pending = 0,
        Approved = 1
    }
}