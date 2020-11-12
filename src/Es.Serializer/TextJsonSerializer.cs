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
#if DEBUG
        private static readonly bool WriteIndented = true;
#else
        private static readonly bool WriteIndented = false;
#endif

        /// <summary>
        /// TextJsonSerializer Instance
        /// </summary>
        public static TextJsonSerializer Instance = new TextJsonSerializer();

        private readonly JsonSerializerOptions _readerOptions;
        private readonly JsonSerializerOptions _writerOptions;

        public TextJsonSerializer() : this(WriteIndented)
        {
        }

        /// <summary>
        /// TextJsonSerializer
        /// </summary>
        public TextJsonSerializer(bool writeIndented = false) : this(new JsonSerializerOptions
        {
            WriteIndented = writeIndented,
            
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
#if NET5_0
            IncludeFields = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.WriteAsString,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
#else
            IgnoreNullValues = true,
#endif
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
        /// <param name="writerOptions"></param>
        /// <param name="readerOptions"></param>
        public TextJsonSerializer(JsonSerializerOptions writerOptions, JsonSerializerOptions readerOptions)
        {
            _writerOptions = writerOptions ?? throw new ArgumentNullException(nameof(writerOptions));
            _readerOptions = readerOptions ?? throw new ArgumentNullException(nameof(readerOptions));
        }

        public override object Deserialize(TextReader reader, Type type)
        {
            return DeserializeFromString(reader.ReadToEnd(), type);
        }

        public override object Deserialize(Stream stream, Type type)
        {
            return JsonSerializer.DeserializeAsync(stream, type, _readerOptions)
                .GetAwaiter().GetResult();
        }

        public override object Deserialize(byte[] data, Type type)
        {
            return JsonSerializer.Deserialize(data, type, _readerOptions);
        }

        public override object DeserializeFromString(string serializedText, Type type)
        {
            return JsonSerializer.Deserialize(serializedText, type, _readerOptions);
        }

        public override void Serialize(object value, TextWriter writer)
        {
            writer.Write(SerializeToString(value));
        }

        public override void Serialize(object value, Stream output)
        {
            using (var writer = new Utf8JsonWriter(output, new JsonWriterOptions
            {
                Indented = _writerOptions.WriteIndented,
                Encoder = _writerOptions.Encoder
            }))
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), _writerOptions);
            }
        }

        public override void Serialize(object value, out byte[] output)
        {
            output = JsonSerializer.SerializeToUtf8Bytes(value, _writerOptions);
        }

        public override string SerializeToString(object value)
        {
            return JsonSerializer.Serialize(value, _writerOptions);
        }
    }
}

#endif