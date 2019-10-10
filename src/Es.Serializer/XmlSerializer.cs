using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Es.Serializer
{
    /// <summary>
    /// XmlSerializer.
    /// </summary>
    public class XmlSerializer : SerializerBase
    {
        private readonly XmlWriterSettings XWSettings = new XmlWriterSettings();
        private readonly XmlReaderSettings XRSettings = new XmlReaderSettings();

        /// <summary>
        /// XmlSerializer Instance
        /// </summary>
        public static XmlSerializer Instance = new XmlSerializer();

        /// <summary>
        /// XmlSerializer
        /// </summary>
        /// <param name="omitXmlDeclaration"></param>
        /// <param name="maxCharsInDocument"></param>
        public XmlSerializer(bool omitXmlDeclaration = false, int maxCharsInDocument = 1024 * 1024)
        {
            XWSettings.Encoding = new UTF8Encoding(false);
            XWSettings.OmitXmlDeclaration = omitXmlDeclaration;
            XRSettings.MaxCharactersInDocument = maxCharsInDocument;
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(TextReader reader, Type type)
        {
            using (var xr = XmlReader.Create(reader, XRSettings))
            {
                var serializer = new System.Runtime.Serialization.DataContractSerializer(type);
                return serializer.ReadObject(xr);
            }
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public override object Deserialize(Stream stream, Type type)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding, true, 1024, true))
                return Deserialize(reader, type);
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
            using (var xw = XmlWriter.Create(writer, XWSettings))
            {
                var serializer = new System.Runtime.Serialization.DataContractSerializer(value.GetType());
                serializer.WriteObject(xw, value);
            }
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public override void Serialize(object value, Stream output)
        {
            using (StreamWriter sw = new StreamWriter(output, Encoding, 1024, true))
                Serialize(value, sw);
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