﻿#if !NET5_0
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Es.Serializer
{
    /// <summary>
    /// BinarySerializer.
    /// </summary>
    public class BinarySerializer : SerializerBase
    {
        /// <summary>
        /// BinarySerializer Instance
        /// </summary>
        public static BinarySerializer Instance = new BinarySerializer();

        /// <summary>
        /// The binary formatter
        /// </summary>
        private readonly IFormatter binaryFormatter = new BinaryFormatter();

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
            return binaryFormatter.Deserialize(stream);
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
            binaryFormatter.Serialize(output, value);
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
                return ToHex(mem.ToArray());
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
            var data = FromHex(serializedText);
            using (MemoryStream mem = new MemoryStream(data))
            {
                return Deserialize(mem, type);
            }
        }
    }
}
#endif