using System;

namespace Es.Serializer
{
    /// <summary>
    /// Interface IStringSerializer
    /// </summary>
    public interface IStringSerializer
    {
        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <param name="serializedText">The serialized text.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        object DeserializeFromString(string serializedText, Type type);

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <returns>System.String.</returns>
        string SerializeToString(object value);
    }
}