// ==++==
//
//  Copyright (c) . All rights reserved.
//
// ==--==
/* ---------------------------------------------------------------------------
 *
 * Author			: v.la
 * Email			: v.la@live.cn
 * Created			: 2015-08-31
 * Class			: NETSerializer.cs
 *
 * ---------------------------------------------------------------------------
 * */

using System;
using System.IO;
using NS = NetSerializer;

namespace Es.Serializer
{
    /// <summary>
    /// Class NETSerializer.
    /// </summary>
    public class NETSerializer : SerializerBase
    {

        /// <summary>
        /// The _serializer
        /// </summary>
        private NS.Serializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NETSerializer"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        public NETSerializer(NS.Serializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public override void Serialize(object value, Stream output)
        {
            _serializer.Serialize(output, value);
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(Stream stream, Type type)
        {
            return _serializer.Deserialize(stream);
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

        /// <summary>
        /// Serializes the core.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        public override void Serialize(object value, TextWriter writer)
        {
            writer.Write(SerializeToString(value));
        }

        /// <summary>
        /// Deserializes the core.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(TextReader reader, Type type)
        {
            var hex = reader.ReadToEnd();
            return DeserializeFromString(hex, type);
        }


        public override object Deserialize(byte[] data, Type type)
        {
            using (var mem = new MemoryStream(data))
            {
                return Deserialize(mem, type);
            }
        }


        public override void Serialize(object value, out byte[] output)
        {
            using (var mem = new MemoryStream())
            {
                Serialize(value, mem);
                output = mem.ToArray();
            }
        }
    }
}