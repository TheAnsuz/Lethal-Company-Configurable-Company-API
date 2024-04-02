using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static ReadonlyDictionary<K, V> AsReadOnly<K, V>(this Dictionary<K, V> dictionary) => new(dictionary);
    }
}
