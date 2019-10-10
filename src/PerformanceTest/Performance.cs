using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Es.Serializer;

namespace PerformanceTest
{
    public class Performance
    {
        public class CallAct
        {
            public Action Act { get; set; }

            public double Score { get; set; }
        }

        private Dictionary<string, CallAct> serializer = new Dictionary<string, CallAct>(StringComparer.OrdinalIgnoreCase)
        {
            {"Json",new CallAct()},
            {"Protobuf",new CallAct()},
            {"Xml",new CallAct()},
            {"Jil",new CallAct()},
            {"Binary",new CallAct()},
            {"DataContract",new CallAct()},
            {"NET",new CallAct()},
        #if NETFULL
            {"Soap",new CallAct()},
           
        #endif
        };

#if NETFULL || NETCOREAPP3_0
        private JilSerializer jilserializer = new JilSerializer();
        private BinarySerializer binaryserializer = new BinarySerializer();
        private DataContractSerializer datacontractserializer = new DataContractSerializer();

#endif

#if NETFULL
        SoapSerializer soapserializer = new SoapSerializer();
#endif

        private JsonNetSerializer jsonnetserializer = new JsonNetSerializer();
        private ProtoBufSerializer protobufserializer = new ProtoBufSerializer();
        private XmlSerializer xmlserializer = new XmlSerializer();

        [Test("Serializers Performance Test")]
        public void SerializersTest()
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;

            const int runs = 10000;

            var foo = Helper.GetFoo();

            CompareSerializers(foo, runs);

            Console.WriteLine("------------------- single\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
                serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());

            var wrapper = new SerializerWrapper(foo);

            CompareSerializers(wrapper, runs);

            Console.WriteLine("------------------- wrapper\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
              serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());
        }

        [Test("Deserialize Performance Test")]
        public void CompareDeserializes()
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;

            const int runs = 10000;

            var foo = Helper.GetFoo();

            CompareDeserializes(foo, runs);

            Console.WriteLine("------------------- single\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
             serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());

            var wrapper = new SerializerWrapper(foo);

            CompareDeserializes(wrapper, runs);

            Console.WriteLine("------------------- wrapper\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
             serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());
        }

        private void CompareSerializers<T>(T obj, int runs)
        {
            //warm-up

#if NETFULL || NETCOREAPP3_0
            jilserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream())
            {
                binaryserializer.Serialize(obj, mem);
            }

            using (MemoryStream mem = new MemoryStream())
            {
                datacontractserializer.Serialize(obj, mem);
            }

#endif

#if NETFULL

            using (MemoryStream mem = new MemoryStream())
            {
                soapserializer.Serialize(obj, mem);
            }

#endif

            var netserializer = SerializerFactory.Get("NET");
            using (MemoryStream mem = new MemoryStream())
            {
                netserializer.Serialize(obj, mem);
            }

            jsonnetserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream())
            {
                protobufserializer.Serialize(obj, mem);
            }
            xmlserializer.SerializeToString(obj);

            var keys = serializer.Keys.ToList();

#if NETFULL || NETCOREAPP3_0
            serializer["Jil"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Jil"].Score = Helper.AverageRuntime(() =>
                {
                    jilserializer.SerializeToString(obj);
                }, runs);
            };
            serializer["Binary"].Act = () =>
             {
                 GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                 serializer["Binary"].Score = Helper.AverageRuntime(() =>
                 {
                     using (MemoryStream mem = new MemoryStream())
                     {
                         binaryserializer.Serialize(obj, mem);
                     }
                 }, runs);
             };

            serializer["DataContract"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["DataContract"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream())
                    {
                        datacontractserializer.Serialize(obj, mem);
                    }
                }, runs);
            };

#endif

#if NETFULL



            serializer["Soap"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Soap"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream())
                    {
                        soapserializer.Serialize(obj, mem);
                    }
                }, runs);
            };

#endif

            serializer["NET"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["NET"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream())
                    {
                        netserializer.Serialize(obj, mem);
                    }
                }, runs);
            };

            serializer["Json"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Json"].Score = Helper.AverageRuntime(() =>
                {
                    jsonnetserializer.SerializeToString(obj);
                }, runs);
            };

            serializer["Xml"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Xml"].Score = Helper.AverageRuntime(() =>
                {
                    xmlserializer.SerializeToString(obj);
                }, runs);
            };

            serializer["Protobuf"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Protobuf"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream())
                    {
                        protobufserializer.Serialize(obj, mem);
                    }
                }, runs);
            };

            keys.ForEach(k =>
            {
                serializer[k].Act();
            });
        }

        private void CompareDeserializes<T>(T obj, int runs)
        {
            var objType = obj.GetType();

            //warm-up

            byte[] protobufData, binaryData, dataContractData, soapData, netData;

#if NETFULL || NETCOREAPP3_0
            var jilSerializedText = jilserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream())
            {
                binaryserializer.Serialize(obj, mem);
                binaryData = mem.ToArray();
            }

            using (MemoryStream mem = new MemoryStream())
            {
                datacontractserializer.Serialize(obj, mem);
                dataContractData = mem.ToArray();
            }
#endif

#if NETFULL

            using (MemoryStream mem = new MemoryStream())
            {
                soapserializer.Serialize(obj, mem);
                soapData = mem.ToArray();
            }
#endif

            var netserializer = SerializerFactory.Get("NET");
            using (MemoryStream mem = new MemoryStream())
            {
                netserializer.Serialize(obj, mem);
                netData = mem.ToArray();
            }

            var jsonnetSerializedText = jsonnetserializer.SerializeToString(obj);
            var xmlSerializedText = xmlserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream())
            {
                protobufserializer.Serialize(obj, mem);
                protobufData = mem.ToArray();
            }

            var keys = serializer.Keys.ToList();

#if NETFULL || NETCOREAPP3_0
            serializer["Jil"].Act = () =>
                      {
                          GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                          serializer["Jil"].Score = Helper.AverageRuntime(() =>
                          {
                              jilserializer.DeserializeFromString(jilSerializedText, objType);
                          }, runs);
                      };
            serializer["Binary"].Act = () =>
          {
              GC.Collect(2, GCCollectionMode.Forced, blocking: true);
              serializer["Binary"].Score = Helper.AverageRuntime(() =>
              {
                  using (MemoryStream mem = new MemoryStream(binaryData))
                  {
                      binaryserializer.Deserialize(mem, objType);
                  }
              }, runs);
          };

            serializer["DataContract"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["DataContract"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(dataContractData))
                    {
                        datacontractserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };

#endif

#if NETFULL

            serializer["Soap"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Soap"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(soapData))
                    {
                        soapserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };
#endif

            serializer["NET"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["NET"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(netData))
                    {
                        netserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };

            serializer["Json"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Json"].Score = Helper.AverageRuntime(() =>
                {
                    jsonnetserializer.DeserializeFromString(jsonnetSerializedText, objType);
                }, runs);
            };

            serializer["Xml"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Xml"].Score = Helper.AverageRuntime(() =>
                {
                    xmlserializer.DeserializeFromString(xmlSerializedText, objType);
                }, runs);
            };

            serializer["Protobuf"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Protobuf"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(protobufData))
                    {
                        protobufserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };

            keys.ForEach(k =>
            {
                serializer[k].Act();
            });
        }
    }
}