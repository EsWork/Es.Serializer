﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PerformanceTest
{
    [Serializable, DataContract, ProtoContract]
    public class Foo
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

    [Serializable, DataContract, ProtoContract]
    public class InnerFoo
    {
        [DataMember, ProtoMember(1)]
        public decimal InnerNumeric {
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