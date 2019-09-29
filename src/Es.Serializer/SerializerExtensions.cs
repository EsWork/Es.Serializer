

using System.IO;

namespace Es.Serializer
{
    /// <summary>
    /// Class SerializerExtensions.
    /// </summary>
    public static class SerializerExtensions
    {
        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>T.</returns>
        public static T Deserialize<T>(this ObjectSerializerBase self, Stream stream) {
            return (T)self.Deserialize(stream, typeof(T));
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="To">The type of to.</typeparam>
        /// <param name="self">The self.</param>
        /// <param name="data">The data.</param>
        /// <returns>To.</returns>
        public static To Deserialize<To>(this IObjectSerializer self, byte[] data) {
            return (To)self.Deserialize(data, typeof(To));
        }

        /// <summary>
        /// Deserializes the specified reader.
        /// </summary>
        /// <typeparam name="To">The type of to.</typeparam>
        /// <param name="self">The self.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>To.</returns>
        public static To Deserialize<To>(this IObjectSerializer self, TextReader reader) {
            return (To)self.Deserialize(reader, typeof(To));
        }

        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <typeparam name="To">The type of to.</typeparam>
        /// <param name="self">The self.</param>
        /// <param name="serializedText">The serialized text.</param>
        /// <returns>To.</returns>
        public static To DeserializeFromString<To>(this ObjectSerializerBase self, string serializedText) {
            return (To)self.DeserializeFromString(serializedText, typeof(To));
        }

        /// <summary>
        /// Deeps the clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <param name="obj">The object.</param>
        /// <returns>T.</returns>
        public static T DeepClone<T>(this ObjectSerializerBase self, T obj) {
            using (MemoryStream ms = new MemoryStream()) {
                self.Serialize(obj, ms);
                ms.Seek(0, SeekOrigin.Begin);
                return self.Deserialize<T>(ms);
            }
        }

        /// <summary>
        /// Deeps the clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>T.</returns>
        public static T DeepClone<T>(this T obj) {
            return (T)XmlSerializer.Instance.DeepClone(obj);
        }

        /// <summary>
        /// Serialize To Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj)
        {
            return XmlSerializer.Instance.SerializeToString(obj);
        }

        /// <summary>
        /// Deserialize From Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string xml)
        {
            return XmlSerializer.Instance.DeserializeFromString<T>(xml);
        }
    }
}