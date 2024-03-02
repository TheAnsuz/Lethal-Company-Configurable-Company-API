using Amrv.ConfigurableCompany.API.Display;
using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API
{
    public abstract class CType
    {
        private static readonly Dictionary<Type, CType> _mapping = [];

        /// <summary>
        /// Tries to get the CType that corresponds to the provided type
        /// </summary>
        /// <typeparam name="T">The type of parameter to lookup</typeparam>
        /// <param name="ctype">The configuration type that corresponds to the provided generic</param>
        /// <returns>True if there was a mapping, false is no one was found</returns>
        public static bool TryGetMapping<T>(out CType ctype) => _mapping.TryGetValue(typeof(T), out ctype);
        /// <summary>
        /// Tries to get the CType that corresponds to the provided type
        /// </summary>
        /// <param name="type">The type of parameter to lookup</param>
        /// <param name="ctype">The configuration type that corresponds to the provided generic</param>
        /// <returns>True if there was a mapping, false is no one was found</returns>
        public static bool TryGetMapping(Type type, out CType ctype) => _mapping.TryGetValue(type, out ctype);

        /// <summary>
        /// Sets the mapping for a specific type to the provided CType.
        /// <para></para>
        /// By default it will replace the mapping if it already exists, you can disable that option.
        /// </summary>
        /// <typeparam name="T">The type to mapping</typeparam>
        /// <param name="ctype">The CType to map for this type</param>
        /// <param name="replace">If any found mapping should be replaced or not</param>
        public static void SetMapping<T>(CType ctype, bool replace = true)
        {
            if (replace)
                _mapping[typeof(T)] = ctype;

            else if (!_mapping.ContainsKey(typeof(T)))
                _mapping[typeof(T)] = ctype;
        }

        /// <summary>
        /// The default value that will be used for this type as a fallback value
        /// </summary>
        public abstract object Default { get; }

        /// <summary>
        /// The name that will be displayed in the in-game menu
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// Should return an instance of a configuration display that will represent an instance of a configuration that uses this configuration type.
        /// </summary>
        /// <param name="config">The configuration that will be created</param>
        /// <returns>An instance of the configuration display that will be shown in-game</returns>
        protected internal abstract ConfigDisplay CreateDisplay { get; }

        /// <summary>
        /// Checks if the provided value is valid to be used with this configuration type
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True if can be accepted as a value, false otherwise</returns>
        public abstract bool IsValidValue(object value);

        /// <summary>
        /// Tries to convert a value to an accepted value by this configuration type
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="result">The converted value</param>
        /// <param name="formatProvider">The machine-specific format for the conversion</param>
        /// <returns>True if the conversion succeded, false otherwise</returns>
        public abstract bool TryConvert(object value, out object result, IFormatProvider formatProvider = null);

        /// <summary>
        /// Tries to convert a value (should match the accepted types of the CType) to other type of value.
        /// <para></para>
        /// The returned value should represent the original value and should return the original value if it is converted back.
        /// </summary>
        /// <typeparam name="T">The type that the value will be converted into</typeparam>
        /// <param name="value">The value that will be converted</param>
        /// <param name="result">The converted result</param>
        /// <returns>True if the conversion succeded, false otherwise</returns>
        public virtual bool TryGetAs<T>(object value, out T result) => TryGetAs(value, out result, typeof(T), Type.GetTypeCode(typeof(T)));

        /// <summary>
        /// Tries to convert a value (should match the accepted types of the CType) to other type of value.
        /// <para></para>
        /// The returned value should represent the original value and should return the original value if it is converted back.
        /// </summary>
        /// <typeparam name="T">The type that the value will be converted into</typeparam>
        /// <param name="value">The value that will be converted</param>
        /// <param name="result">The converted result</param>
        /// <param name="type">The type of the resulting type</param>
        /// <param name="code">The TypeCode of the resulting type</param>
        /// <param name="formatProvider">THe format used for machine-specific conversions</param>
        /// <returns>True if the conversion succeded, false otherwise</returns>
        public abstract bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null);

        /// <summary>
        /// The process to revert back a serialized string to a value that is acceptable by this configuration type.
        /// <para></para>
        /// </summary>
        /// <param name="data">The serialized data to be recovered</param>
        /// <param name="item">The resulting value</param>
        /// <returns>True if the conversion succeded, false otherwise</returns>
        protected internal abstract bool Deserialize(in string data, out object item);

        /// <summary>
        /// The process to convert a value represented by this configuration type to a string that contains the value.
        /// <para></para>
        /// The string should not be machine-specific.
        /// </summary>
        /// <param name="item">The value to be converted into a string</param>
        /// <param name="data">The result of the conversion</param>
        /// <returns>True if the conversion succeded, false otherwise</returns>
        protected internal abstract bool Serialize(in object item, out string data);

    }
}
