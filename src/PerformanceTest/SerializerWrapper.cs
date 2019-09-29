using System;
using System.Runtime.Serialization;
using Es.Serializer;
using Newtonsoft.Json;
using ProtoBuf;

namespace PerformanceTest
{
#if NETFULL || NETCOREAPP3_0
    [Serializable]
#endif
    [ProtoContract]
    public class SerializerWrapper
    {

        private static ProtoBufSerializer _protoBufSerializer = new ProtoBufSerializer();
#if NETFULL || NETCOREAPP3_0
        [NonSerialized]
#endif
        [IgnoreDataMember, JsonIgnore, ProtoIgnore]
        private object _value;

        public SerializerWrapper() {
        }

        public SerializerWrapper(object value) {
            ValueType = value.GetType().AssemblyQualifiedName;
            _value = value;
            JsonString = _protoBufSerializer.SerializeToString(value);
        }

        [ProtoMember(1)]
        public string ValueType { get; set; }

        [ProtoMember(2)]
        public string JsonString { get; set; }

        [IgnoreDataMember, JsonIgnore, ProtoIgnore]
        public object Value
        {
            get
            {
                if (_value == null && !string.IsNullOrEmpty(JsonString))
                    _value = _protoBufSerializer
                        .DeserializeFromString(JsonString,
                         Type.GetType(ValueType, true, true));
                return _value;
            }
        }
    }
}