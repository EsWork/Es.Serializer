using System;
using System.IO;

namespace Es.Serializer
{
    /// <summary>
    /// Class ObjectSerializerBase.
    /// </summary>
    [Obsolete("This type is obsolete and please use SerializerBase")]
    public abstract class ObjectSerializerBase : SerializerBase
    {
        /// <summary>
        /// Serializes the core.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        protected virtual void SerializeCore(object value, TextWriter writer)
        {
            Serialize(value, writer);
        }

        /// <summary>
        /// Deserializes the core.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        protected virtual object DeserializeCore(TextReader reader, Type type)
        {
            return Deserialize(reader, type);
        }
    }
}