using System.Collections;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.Core
{
    public sealed class ReadonlyDictionary<K, V>(Dictionary<K, V> wrapper) : IReadOnlyDictionary<K, V>
    {
        public V this[K key] => wrapper.GetValueOrDefault(key, default);

        public IEnumerable<K> Keys => wrapper.Keys;

        public IEnumerable<V> Values => wrapper.Values;

        public int Count => wrapper.Count;

        public bool ContainsKey(K key) => wrapper.ContainsKey(key);

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => wrapper.GetEnumerator();

        public bool TryGetValue(K key, out V value) => wrapper.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => wrapper.GetEnumerator();
    }
}
