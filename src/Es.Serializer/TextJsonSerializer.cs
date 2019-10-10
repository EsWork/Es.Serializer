#if !NET45

using System;
using System.IO;
using System.Text.Json;

namespace Es.Serializer
{
    /// <summary>
    /// System.Text.Json Serializer
    /// </summary>
    public class TextJsonSerializer : SerializerBase
    {
        private readonly JsonSerializerOptions _options;

        /// <summary>
        /// TextJsonSerializer
        /// </summary>
        public TextJsonSerializer() : this(new JsonSerializerOptions
        {
            WriteIndented = false,
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        })
        {
        }

        /// <summary>
        /// TextJsonSerializer
        /// </summary>
        public TextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override object Deserialize(TextReader reader, Type type)
        {
            return DeserializeFromString(reader.ReadToEnd(), type);
        }

        public override object Deserialize(Stream stream, Type type)
        {
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                return Deserialize(mem.ToArray(), type);
            }
        }

        public override object Deserialize(byte[] data, Type type)
        {
            return JsonSerializer.Deserialize(data, type, _options);
        }

        public override object DeserializeFromString(string serializedText, Type type)
        {
            return JsonSerializer.Deserialize(serializedText, type, _options);
        }

        public override void Serialize(object value, TextWriter writer)
        {
            writer.Write(SerializeToString(value));
        }

        public override void Serialize(object value, Stream output)
        {
            using (var writer = new Utf8JsonWriter(output))
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), _options);
            }
        }

        public override void Serialize(object value, out byte[] output)
        {
            output = JsonSerializer.SerializeToUtf8Bytes(value, _options);
        }

        public override string SerializeToString(object value)
        {
            return JsonSerializer.Serialize(value, _options);
        }
    }
}

#endif