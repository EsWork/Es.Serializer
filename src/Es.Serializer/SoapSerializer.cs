﻿// ==++== 
//
//  Copyright (c) . All rights reserved.
//
// ==--== 
/* ---------------------------------------------------------------------------
 *
 * Author			: v.la 
 * Email			: v.la@live.cn
 * Created			: 2015-08-31
 * Class			: SoapSerializer.cs
 *
 * ---------------------------------------------------------------------------
 * */
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace Es.Serializer
{
    /// <summary>
    /// Class SoapSerializer.
    /// </summary>
    public class SoapSerializer : ObjectSerializerBase
    {
        /// <summary>
        /// The binary formatter
        /// </summary>
        private readonly IFormatter binaryFormatter = new SoapFormatter();

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public override void Serialize(object value, Stream output) {
            binaryFormatter.Serialize(output, value);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(Stream stream, Type type) {
            return binaryFormatter.Deserialize(stream);
        }

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public override string SerializeToString(object value) {
            using (MemoryStream mem = new MemoryStream()) {
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
        public override object DeserializeFromString(string serializedText, Type type) {
            var data = Encoding.GetBytes(serializedText);
            using (MemoryStream mem = new MemoryStream(data)) {
                return Deserialize(mem, type);
            }
        }

        /// <summary>
        /// Serializes the core.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        protected override void SerializeCore(object value, TextWriter writer) {
            writer.Write(SerializeToString(value));
        }

        /// <summary>
        /// Deserializes the core.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        protected override object DeserializeCore(TextReader reader, Type type) {
            var serializedText = reader.ReadToEnd();
            return DeserializeFromString(serializedText, type);
        }
    }
}