using System;
using System.Collections.Generic;
using System.Threading;

namespace Es.Serializer
{
    /// <summary>
    /// Class SerializerFactory.
    /// </summary>
    public sealed class SerializerFactory
    {
        private static readonly Dictionary<string, SerializerBase> _objectSerializerCache;

        private static SerializerBase _default;

        /// <summary>
        /// Initializes static members of the <see cref="SerializerFactory"/> class.
        /// </summary>
        static SerializerFactory()
        {
            _objectSerializerCache = new Dictionary<string, SerializerBase>(StringComparer.OrdinalIgnoreCase)
            {
#if NETFULL
                ["soap"] = SoapSerializer.Instance,
#endif
                ["binary"] = BinarySerializer.Instance,
                ["DataContract"] = DataContractSerializer.Instance,
                ["dc"] = DataContractSerializer.Instance,
                ["xml"] = XmlSerializer.Instance
            };

            _default = XmlSerializer.Instance;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public static IEnumerable<string> Alias {
            get {
                return _objectSerializerCache.Keys;
            }
        }

        /// <summary>
        /// Gets the specified alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>SerializerBase.</returns>
        public static SerializerBase Get(string alias)
        {
            return _objectSerializerCache[alias];
        }

        /// <summary>
        /// Checks for serialization based on the specified alias
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static bool Contains(string alias)
        {
            return _objectSerializerCache.ContainsKey(alias);
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static SerializerBase Default {
            get { return _default; }
        }

        /// <summary>
        /// Sets the default.
        /// </summary>
        /// <param name="def">The definition.</param>
        /// <returns>SerializerBase.</returns>
        public static SerializerBase SetDefault(SerializerBase def)
        {
            SerializerBase oldValue, currentValue;
            currentValue = _default;
            do
            {
                oldValue = currentValue;
                currentValue = Interlocked.CompareExchange(ref _default, def, oldValue);
            }
            while (currentValue != oldValue);
            return _default;
        }

        /// <summary>
        /// Adds the serializer.
        /// </summary>
        /// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
        /// <param name="alias">The alias.</param>
        public static void AddSerializer<TSerializer>(string alias)
           where TSerializer : SerializerBase
        {
            AddSerializer<TSerializer>(new[] { alias });
        }

        /// <summary>
        /// Adds the serializer.
        /// </summary>
        /// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="alias">The alias.</param>
        public static void AddSerializer<TSerializer>(TSerializer instance, string alias)
           where TSerializer : SerializerBase
        {
            AddSerializer(instance, new[] { alias });
        }

        /// <summary>
        /// Adds the serializer.
        /// </summary>
        /// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
        /// <param name="alias">The alias.</param>
        public static void AddSerializer<TSerializer>(string[] alias)
          where TSerializer : SerializerBase
        {
            var instance = Activator.CreateInstance(typeof(TSerializer)) as SerializerBase;
            AddSerializer(instance, alias);
        }

        /// <summary>
        /// Adds the serializer.
        /// </summary>
        /// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="alias">The alias.</param>
        public static void AddSerializer<TSerializer>(TSerializer instance, string[] alias)
          where TSerializer : SerializerBase
        {
            lock (_objectSerializerCache)
            {
                foreach (var name in alias)
                    _objectSerializerCache[name] = instance;
            }
        }
    }
}