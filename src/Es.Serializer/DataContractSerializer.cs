using System;
using System.IO;
using DCS = System.Runtime.Serialization.DataContractSerializer;

namespace Es.Serializer
{
    /// <summary>
    /// Class DataContractSerializer.
    /// </summary>
    public class DataContractSerializer : SerializerBase
    {
        /// <summary>
        /// DataContractSerializer Instance
        /// </summary>
        public static DataContractSerializer Instance = new DataContractSerializer();

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(TextReader reader, Type type)
        {
            var hex = reader.ReadToEnd();
            return DeserializeFromString(hex, type);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(Stream stream, Type type)
        {
            DCS serializer = new DCS(type);
            return serializer.ReadObject(stream);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(byte[] data, Type type)
        {
            using (var mem = new MemoryStream(data))
            {
                return Deserialize(mem, type);
            }
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        public override void Serialize(object value, TextWriter writer)
        {
            writer.Write(SerializeToString(value));
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public override void Serialize(object value, Stream output)
        {
            DCS serializer = new DCS(value.GetType());
            serializer.WriteObject(output, value);
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public override void Serialize(object value, out byte[] output)
        {
            using (var mem = new MemoryStream())
            {
                Serialize(value, mem);
                output = mem.ToArray();
            }
        }

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public override string SerializeToString(object value)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serialize(value, mem);
                return Encoding.GetString(mem.ToArray());
            }
        }

        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <param name="serializedText">The serialized text.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object DeserializeFromString(string serializedText, Type type)
        {
            var data = Encoding.GetBytes(serializedText);
            using (MemoryStream mem = new MemoryStream(data))
            {
                return Deserialize(mem, type);
            }
        }
    }
}