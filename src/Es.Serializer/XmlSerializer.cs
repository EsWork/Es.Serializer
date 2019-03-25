// ==++==
//
//  Copyright (c) . All rights reserved.
//
// ==--==
/* ---------------------------------------------------------------------------
 *
 * Author			: v.la
 * Email			: v.la@live.cn
 * Created			: 2015-08-27
 * Class			: XmlSerializer.cs
 *
 * ---------------------------------------------------------------------------
 * */

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Es.Serializer
{
    /// <summary>
    /// Class XmlSerializer.
    /// </summary>
    public class XmlSerializer : ObjectSerializerBase
    {
        private readonly XmlWriterSettings XWSettings = new XmlWriterSettings();
        private readonly XmlReaderSettings XRSettings = new XmlReaderSettings();

        /// <summary>
        /// XmlSerializer Instance
        /// </summary>
        public static XmlSerializer Instance = new XmlSerializer();

        /// <summary>
        /// XmlSerializer
        /// </summary>
        /// <param name="omitXmlDeclaration"></param>
        /// <param name="maxCharsInDocument"></param>
        public XmlSerializer(bool omitXmlDeclaration = false, int maxCharsInDocument = 1024 * 1024)
        {
            XWSettings.Encoding = new UTF8Encoding(false);
            XWSettings.OmitXmlDeclaration = omitXmlDeclaration;
            XRSettings.MaxCharactersInDocument = maxCharsInDocument;
        }

        /// <summary>
        /// Serializes the core.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        protected override void SerializeCore(object value, TextWriter writer)
        {
            using (var xw = XmlWriter.Create(writer, XWSettings))
            {
                var serializer = new System.Runtime.Serialization.DataContractSerializer(value.GetType());
                serializer.WriteObject(xw, value);
            }
        }

        /// <summary>
        /// Deserializes the core.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        protected override object DeserializeCore(TextReader reader, Type type)
        {
            using (var xr = XmlReader.Create(reader, XRSettings))
            {
                var serializer = new System.Runtime.Serialization.DataContractSerializer(type);
                return serializer.ReadObject(xr);
            }
        }
    }
}