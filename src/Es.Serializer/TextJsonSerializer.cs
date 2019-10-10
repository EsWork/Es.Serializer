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
        /// <summary>
        /// TextJsonSerializer Instance
        /// </summary>
        public static TextJsonSerializer Instance = new TextJsonSerializer();

        private readonly JsonSerializerOptions _deserializeOptions;
        private readonly JsonSerializerOptions _serializeOptions;

        /// <summary>
        /// TextJsonSerializer
        /// </summary>
        public TextJsonSerializer() : this(new JsonSerializerOptions
        {
            WriteIndented = false,
            IgnoreNullValues = true,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        })
        {
        }

        /// <summary>
        /// TextJsonSerializer
        /// </summary>
        /// <param name="options"></param>
        public TextJsonSerializer(JsonSerializerOptions options) : this(options, options)
        {
        }

        /// <summary>
        /// TextJsonSerializer
        /// </summary>
        /// <param name="serializeOptions"></param>
        /// <param name="deserializeOptions"></param>
        public TextJsonSerializer(JsonSerializerOptions serializeOptions, JsonSerializerOptions deserializeOptions)
        {
            _serializeOptions = serializeOptions ?? throw new ArgumentNullException(nameof(serializeOptions));
            _deserializeOptions = deserializeOptions ?? throw new ArgumentNullException(nameof(deserializeOptions));
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
            return JsonSerializer.Deserialize(data, type, _deserializeOptions);
        }

        public override object DeserializeFromString(string serializedText, Type type)
        {
            return JsonSerializer.Deserialize(serializedText, type, _deserializeOptions);
        }

        public override void Serialize(object value, TextWriter writer)
        {
            writer.Write(SerializeToString(value));
        }

        public override void Serialize(object value, Stream output)
        {
            using (var writer = new Utf8JsonWriter(output))
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), _serializeOptions);
            }
        }

        public override void Serialize(object value, out byte[] output)
        {
            output = JsonSerializer.SerializeToUtf8Bytes(value, _serializeOptions);
        }

        public override string SerializeToString(object value)
        {
            return JsonSerializer.Serialize(value, _serializeOptions);
        }
    }
}

#endif