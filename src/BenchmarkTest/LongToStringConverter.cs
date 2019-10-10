using System;
using System.Globalization;
using Newtonsoft.Json;

namespace BenchmarkTest
{
    public class LongToStringConverter : JsonConverter
    {
        private static Type longType = typeof(long);
        private static Type long2Type = typeof(long?);
        private static Type ulongType = typeof(ulong);
        private static Type ulong2Type = typeof(ulong?);

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsEnum)
                return false;


            return  objectType == longType || objectType == long2Type || objectType == ulongType || objectType == ulong2Type;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            try
            {
                if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer)
                {
                    string val = reader.Value.ToString();

                    if (string.IsNullOrWhiteSpace(val))
                    {
                        return 0L;
                    }

                    if (objectType == ulongType || objectType == ulong2Type)
                    {
                        if (ulong.TryParse(val, out ulong num))
                        {
                            return num;
                        }
                    }

                    if (objectType == longType || objectType == long2Type)
                    {
                        if (long.TryParse(val, out long num))
                        {
                            return num;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error converting value {0} to type '{1}'.", CultureInfo.InvariantCulture, reader.Value.ToString(), objectType), ex);
            }

            // we don't actually expect to get here.
            throw new Exception(string.Format("Unexpected token {0} when parsing enum.", CultureInfo.InvariantCulture, reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue(0);
                return;
            }

            writer.WriteValue(value.ToString());
        }
    }
}