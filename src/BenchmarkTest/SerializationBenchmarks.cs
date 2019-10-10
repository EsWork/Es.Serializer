using System;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Es.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BenchmarkTest
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class SerializationBenchmarks
    {
        private string jsonFromString;
        private string jilFromString;
        private string dataContractFromString;
        private string binaryFromString;
        private string protoBufFromString;
        private string netSerializerFromString;

        private object serializeToStringObject;
        private TestClass serializeToStringClass;

        private TextJsonSerializer textJsonSerializer;
        private JsonNetSerializer jsonNetSerializer;
        private DataContractSerializer dataContractSerializer;
        private BinarySerializer binarySerializer;
        private ProtoBufSerializer protoBufSerializer;
        private JilSerializer jilSerializer;
        private NETSerializer netSerializer;


        [GlobalSetup]
        public void Setup()
        {
            var jsonNetOptions = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Converters = new JsonConverter[] {
                    new IsoDateTimeConverter { DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss" },
                    new LongToStringConverter(),
                },

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                IgnoreNullValues = false,
                PropertyNameCaseInsensitive = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            serializeOptions.Converters.Add(new ImplicitInt64Converter());
            serializeOptions.Converters.Add(new ImplicitUInt64Converter());
            serializeOptions.Converters.Add(new ImplicitDateTimeConverter());
            serializeOptions.Converters.Add(new ImplicitDateTimeOffsetConverter());

            var deserializeOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            deserializeOptions.Converters.Add(new ImplicitInt16Converter());
            deserializeOptions.Converters.Add(new ImplicitUInt16Converter());
            deserializeOptions.Converters.Add(new ImplicitInt32Converter());
            deserializeOptions.Converters.Add(new ImplicitUInt32Converter());
            deserializeOptions.Converters.Add(new ImplicitInt64Converter());
            deserializeOptions.Converters.Add(new ImplicitUInt64Converter());
            deserializeOptions.Converters.Add(new ImplicitDecimalConverter());
            deserializeOptions.Converters.Add(new ImplicitDoubleConverter());
            deserializeOptions.Converters.Add(new ImplicitSingleConverter());
            deserializeOptions.Converters.Add(new ImplicitByteConverter());
            deserializeOptions.Converters.Add(new ImplicitSByteConverter());
            deserializeOptions.Converters.Add(new ImplicitDateTimeConverter());
            deserializeOptions.Converters.Add(new ImplicitDateTimeOffsetConverter());

            serializeToStringObject = new
            {
                int16 = 111.ToString(),
                uint16 = ushort.MaxValue.ToString(),
                int32 = int.MaxValue.ToString(),
                uint32 = uint.MaxValue.ToString(),
                int32N = "".ToString(),
                int64 = 12321.ToString(),
                uint64 = ulong.MaxValue.ToString(),
                boolean = true,
                decimalV = decimal.MaxValue.ToString(),
                doubleV = "1.123445767",
                floatV = "1.1111",
                byteV = byte.MaxValue.ToString(),
                sbyteV = sbyte.MaxValue.ToString(),
                charV = 'c',
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                arr = new string[] { "a", "b" },
                TestEnum = TestEnum.Approved
            };

            jsonFromString = JsonConvert.SerializeObject(serializeToStringObject, jsonNetOptions);
          
            serializeToStringClass = (TestClass)JsonConvert.DeserializeObject(jsonFromString, typeof(TestClass), jsonNetOptions);

            jsonNetSerializer = new JsonNetSerializer(jsonNetOptions);
            textJsonSerializer = new TextJsonSerializer(serializeOptions, deserializeOptions);
            jilSerializer = new JilSerializer();

            dataContractSerializer = new DataContractSerializer();
            binarySerializer = new BinarySerializer();
            protoBufSerializer = new ProtoBufSerializer();

            NetSerializer.Serializer instance = new NetSerializer.Serializer(new[] { typeof(TestClass) });
            netSerializer = new NETSerializer(instance);

            dataContractFromString = dataContractSerializer.SerializeToString(serializeToStringClass);
            binaryFromString = binarySerializer.SerializeToString(serializeToStringClass);
            protoBufFromString = protoBufSerializer.SerializeToString(serializeToStringClass);
            netSerializerFromString = netSerializer.SerializeToString(serializeToStringClass);

            jilFromString = jilSerializer.SerializeToString(serializeToStringClass);

        }

        [Benchmark]
        public void SerializeToString_TextJson()
        {
            textJsonSerializer.SerializeToString(serializeToStringObject);
        }

        [Benchmark]
        public void SerializeToString_Newtonsoft()
        {
            jsonNetSerializer.SerializeToString(serializeToStringObject);
        }

        [Benchmark]
        public void SerializeToString_Jil()
        {
            jilSerializer.SerializeToString(serializeToStringObject);
        }

        [Benchmark]
        public void SerializeToString_DataContract()
        {
            dataContractSerializer.SerializeToString(serializeToStringClass);
        }

        [Benchmark]
        public void SerializeToString_Binary()
        {
            binarySerializer.SerializeToString(serializeToStringClass);
        }

        [Benchmark]
        public void SerializeToString_ProtoBuf()
        {
            protoBufSerializer.SerializeToString(serializeToStringClass);
        }

        [Benchmark]
        public void SerializeToString_NetSerializer()
        {
            netSerializer.SerializeToString(serializeToStringClass);
        }

        [Benchmark]
        public void DeserializeFromString_TextJson()
        {
            textJsonSerializer.DeserializeFromString<TestClass>(jsonFromString);
        }

        [Benchmark]
        public void DeserializeFromString_Newtonsoft()
        {
            jsonNetSerializer.DeserializeFromString<TestClass>(jsonFromString);
        }

        [Benchmark]
        public void DeserializeFromString_Jil()
        {
            jilSerializer.DeserializeFromString<TestClass>(jilFromString);
        }

        [Benchmark]
        public void DeserializeFromString_DataContract()
        {
            dataContractSerializer.DeserializeFromString<TestClass>(dataContractFromString);
        }

        [Benchmark]
        public void DeserializeFromString_Binary()
        {
            binarySerializer.DeserializeFromString<TestClass>(binaryFromString);
        }

        [Benchmark]
        public void DeserializeFromString_ProtoBuf()
        {
            protoBufSerializer.DeserializeFromString<TestClass>(protoBufFromString);
        }

        [Benchmark]
        public void DeserializeFromString_NetSerializer()
        {
            netSerializer.DeserializeFromString<TestClass>(netSerializerFromString);
        }
    }
}