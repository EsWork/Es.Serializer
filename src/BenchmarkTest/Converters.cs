using System;
using System.Buffers;
using System.Buffers.Text;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BenchmarkTest
{
    public class ImplicitInt16Converter : JsonConverter<short>
    {
        private const short Zero = 0;

        public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out short number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (short.TryParse(stringValue, out short value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt16();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitUInt16Converter : JsonConverter<ushort>
    {
        private const ushort Zero = 0;

        public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out ushort number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (ushort.TryParse(stringValue, out ushort value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetUInt16();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitInt32Converter : JsonConverter<int>
    {
        private const int Zero = 0;

        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitUInt32Converter : JsonConverter<uint>
    {
        private const uint Zero = 0;

        public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out uint number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (uint.TryParse(stringValue, out uint value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetUInt32();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitInt64Converter : JsonConverter<long>
    {
        private const long Zero = 0L;

        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out long number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (long.TryParse(stringValue, out long value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class ImplicitUInt64Converter : JsonConverter<ulong>
    {
        private const ulong Zero = 0UL;

        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out ulong number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (ulong.TryParse(stringValue, out ulong value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetUInt64();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class ImplicitDecimalConverter : JsonConverter<decimal>
    {
        private const decimal Zero = 0M;

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out decimal number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (decimal.TryParse(stringValue, out decimal value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitDoubleConverter : JsonConverter<double>
    {
        private const double Zero = 0D;

        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out double number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (double.TryParse(stringValue, out double value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDouble();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitSingleConverter : JsonConverter<float>
    {
        private const float Zero = 0F;

        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out float number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (float.TryParse(stringValue, out float value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSingle();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitByteConverter : JsonConverter<byte>
    {
        private const byte Zero = 0;

        public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out byte number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (byte.TryParse(stringValue, out byte value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetByte();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitSByteConverter : JsonConverter<sbyte>
    {
        private const sbyte Zero = 0;

        public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Zero;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out sbyte number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (sbyte.TryParse(stringValue, out sbyte value))
                {
                    return value;
                }

                return Zero;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSByte();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class ImplicitDateTimeConverter : JsonConverter<DateTime>
    {
        private const string FORMAT = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return DateTime.MinValue;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out DateTime number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (DateTime.TryParse(stringValue, out DateTime value))
                {
                    return value;
                }
            }

            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(FORMAT, CultureInfo.CurrentCulture));
        }
    }

    public class ImplicitDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        private const string FORMAT = "yyyy-MM-dd HH:mm:ss";

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return DateTimeOffset.MinValue;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out DateTimeOffset number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                string stringValue = reader.GetString();
                if (DateTimeOffset.TryParse(stringValue, out DateTimeOffset value))
                {
                    return value;
                }
            }

            return reader.GetDateTimeOffset();
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(FORMAT, CultureInfo.CurrentCulture));
        }
    }
}