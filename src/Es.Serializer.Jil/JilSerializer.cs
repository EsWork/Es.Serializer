using System;
using System.IO;

namespace Es.Serializer
{
    public class JilSerializer : SerializerBase
    {
        private Jil.Options _options;

        /// <summary>
        /// JilSerializer Instance
        /// </summary>
        public static JilSerializer Instance = new JilSerializer();

        public JilSerializer(Jil.Options options)
        {
            _options = options;
        }

        public JilSerializer() : this(new Jil.Options(
               prettyPrint: false,
               excludeNulls: false,
               dateFormat: Jil.DateTimeFormat.ISO8601))
        {
        }

        public override object Deserialize(TextReader reader, Type type)
        {
            return Jil.JSON.Deserialize(reader, type, _options);
        }

        public override object Deserialize(Stream stream, Type type)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding, true, 1024, true))
                return Deserialize(reader, type);
        }

        public override object Deserialize(byte[] data, Type type)
        {
            using (var mem = new MemoryStream(data))
            {
                return Deserialize(mem, type);
            }
        }

        public override void Serialize(object value, TextWriter writer)
        {
            Jil.JSON.Serialize(value, writer, _options);
        }

        public override void Serialize(object value, Stream output)
        {
            using (StreamWriter sw = new StreamWriter(output, Encoding, 1024, true))
                Serialize(value, sw);
        }

        public override void Serialize(object value, out byte[] output)
        {
            using (var mem = new MemoryStream())
            {
                Serialize(value, mem);
                output = mem.ToArray();
            }
        }

        public override string SerializeToString(object value)
        {
            return Jil.JSON.Serialize(value, _options);
        }

        public override object DeserializeFromString(string serializedText, Type type)
        {
            return Jil.JSON.Deserialize(serializedText, type, _options);
        }
    }
}