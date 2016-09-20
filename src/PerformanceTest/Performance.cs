﻿using System;
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

        Dictionary<string, CallAct> serializer = new Dictionary<string, CallAct>(StringComparer.OrdinalIgnoreCase)
        {
            {"Json",new CallAct()},
            {"Protobuf",new CallAct()},
            {"Xml",new CallAct()},
            #if NET45
             {"Jil",new CallAct()},
             {"Binary",new CallAct()},
             {"Soap",new CallAct()},
             {"NET",new CallAct()},
            #endif
            {"DataContract",new CallAct()}
        };

#if NET45
        JilSerializer jilserializer = new JilSerializer();
        BinarySerializer binaryserializer = new BinarySerializer();
        SoapSerializer soapserializer = new SoapSerializer();
#endif

        JsonNetSerializer jsonnetserializer = new JsonNetSerializer();
        ProtoBufSerializer protobufserializer = new ProtoBufSerializer();
        XmlSerializer xmlserializer = new XmlSerializer();

        DataContractSerializer datacontractserializer = new DataContractSerializer();


        [Test("Serializers Performance Test")]
        public void SerializersTest() {

            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;

            const int runs = 10000;

            var foo = Helper.GetFoo();

            CompareSerializers(foo, runs);

            Console.WriteLine("single\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
                serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());

            var wrapper = new SerializerWrapper(foo);

            CompareSerializers(wrapper, runs);

            Console.WriteLine("wrapper\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
              serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());
        }


        [Test("Deserialize Performance Test")]
        public void CompareDeserializes() {

            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;

            const int runs = 10000;

            var foo = Helper.GetFoo();

            CompareDeserializes(foo, runs);

            Console.WriteLine("single\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
             serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());

            var wrapper = new SerializerWrapper(foo);

            CompareDeserializes(wrapper, runs);

            Console.WriteLine("wrapper\r\n" + string.Join("\n", Enumerable.Range(0, serializer.Count).Select(s => "{" + s + "}")),
             serializer.Select(s => s.Key + ":" + s.Value.Score.ToString()).ToArray());

        }

        void CompareSerializers<T>(T obj, int runs) {

            //warm-up

#if NET45
            jilserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream()) {
                binaryserializer.Serialize(obj, mem);
            }
            using (MemoryStream mem = new MemoryStream()) {
                soapserializer.Serialize(obj, mem);
            }

            var netserializer = SerializerFactory.Get("NET");
            using (MemoryStream mem = new MemoryStream()) {
                netserializer.Serialize(obj, mem);
            }
#endif

            jsonnetserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream()) {
                protobufserializer.Serialize(obj, mem);
            }
            xmlserializer.SerializeToString(obj);

            using (MemoryStream mem = new MemoryStream()) {
                datacontractserializer.Serialize(obj, mem);
            }

            var keys = serializer.Keys.ToList();

#if NET45
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
                     using (MemoryStream mem = new MemoryStream()) {
                         binaryserializer.Serialize(obj, mem);
                     }
                 }, runs);
             };

            serializer["Soap"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Soap"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream()) {
                        soapserializer.Serialize(obj, mem);
                    }
                }, runs);
            };

            serializer["NET"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["NET"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream()) {
                        netserializer.Serialize(obj, mem);
                    }
                }, runs);
            };
#endif

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
                    using (MemoryStream mem = new MemoryStream()) {
                        protobufserializer.Serialize(obj, mem);
                    }
                }, runs);
            };



            serializer["DataContract"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["DataContract"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream()) {
                        datacontractserializer.Serialize(obj, mem);
                    }
                }, runs);
            };



            keys.ForEach(k =>
            {
                serializer[k].Act();
            });

        }

        void CompareDeserializes<T>(T obj, int runs) {

            var objType = obj.GetType();

            //warm-up

            byte[] protobufData, binaryData, dataContractData, soapData, netData;

#if NET45
            var jilSerializedText = jilserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream()) {
                binaryserializer.Serialize(obj, mem);
                binaryData = mem.ToArray();
            }
            using (MemoryStream mem = new MemoryStream()) {
                soapserializer.Serialize(obj, mem);
                soapData = mem.ToArray();
            }

            var netserializer = SerializerFactory.Get("NET");
            using (MemoryStream mem = new MemoryStream()) {
                netserializer.Serialize(obj, mem);
                netData = mem.ToArray();
            }
#endif

            var jsonnetSerializedText = jsonnetserializer.SerializeToString(obj);
            var xmlSerializedText = xmlserializer.SerializeToString(obj);
            using (MemoryStream mem = new MemoryStream()) {
                protobufserializer.Serialize(obj, mem);
                protobufData = mem.ToArray();
            }
            using (MemoryStream mem = new MemoryStream()) {
                datacontractserializer.Serialize(obj, mem);
                dataContractData = mem.ToArray();
            }


            var keys = serializer.Keys.ToList();

#if NET45
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
                  using (MemoryStream mem = new MemoryStream(binaryData)) {
                      binaryserializer.Deserialize(mem, objType);
                  }
              }, runs);
          };

            serializer["Soap"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["Soap"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(soapData)) {
                        soapserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };


            serializer["NET"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["NET"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(netData)) {
                        netserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };
#endif



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
                    using (MemoryStream mem = new MemoryStream(protobufData)) {
                        protobufserializer.Deserialize(mem, objType);
                    }
                }, runs);
            };


            serializer["DataContract"].Act = () =>
            {
                GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                serializer["DataContract"].Score = Helper.AverageRuntime(() =>
                {
                    using (MemoryStream mem = new MemoryStream(dataContractData)) {
                        datacontractserializer.Deserialize(mem, objType);
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