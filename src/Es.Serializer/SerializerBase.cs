using System;
using System.IO;
using System.Text;

namespace Es.Serializer
{
    /// <summary>
    /// The serializer abstract class
    /// </summary>
    public abstract class SerializerBase : IObjectSerializer, IStringSerializer
    {
        private static readonly byte[] HexTable =
        {
            (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7',
            (byte)'8', (byte)'9', (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f'
        };


        /// <summary>
        /// The encoding-UTF8
        /// </summary>
        protected readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public abstract object Deserialize(TextReader reader, Type type);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public abstract object Deserialize(Stream stream, Type type);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public abstract object Deserialize(byte[] data, Type type);

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        public abstract void Serialize(object value, TextWriter writer);

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public abstract void Serialize(object value, Stream output);

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="output">The output.</param>
        public abstract void Serialize(object value, out byte[] output);

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public abstract string SerializeToString(object value);

        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <param name="serializedText">The serialized text.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public abstract object DeserializeFromString(string serializedText, Type type);

        /// <summary>
        /// To the hexadecimal.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        protected static string ToHex(byte[] data)
        {
            var length = data.Length;

            var hex = new char[length * 2];

            for (int i = 0, j = 0; i < length; i++, j += 2)
            {
                hex[j] = (char)HexTable[(data[i] >> 4) & 0x0f];
                hex[j + 1] = (char)HexTable[data[i] & 0x0f];
            }

            return new string(hex);
        }

        /// <summary>
        /// Froms the hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns>System.Byte[].</returns>
        protected static byte[] FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return new byte[0];

            var length = hex.Length;

            var data = new byte[length / 2];

            int off = 0;
            for (int read_index = 0; read_index < length; read_index += 2)
            {
                byte upper = FromCharacterToByte((byte)hex[read_index], read_index, 4);
                byte lower = FromCharacterToByte((byte)hex[read_index + 1], read_index + 1);

                data[off++] = (byte)(upper | lower);
            }

            return data;
        }

        private static byte FromCharacterToByte(byte value, int index, int shift = 0)
        {
            if (((0x40 < value) && (0x47 > value)) || ((0x60 < value) && (0x67 > value)))
            {
                if (0x40 == (0x40 & value))
                {
                    if (0x20 == (0x20 & value))
                        value = (byte)(((value + 0xA) - 0x61) << shift);
                    else
                        value = (byte)(((value + 0xA) - 0x41) << shift);
                }
            }
            else if ((0x29 < value) && (0x40 > value))
                value = (byte)((value - 0x30) << shift);
            else
                throw new InvalidOperationException(String.Format("Character '{0}' at index '{1}' is not valid alphanumeric character.", (char)value, index));

            return value;
        }
    }
}