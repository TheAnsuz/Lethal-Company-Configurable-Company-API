using System.Collections;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.Core
{
    public class IDStorage<T> : IReadOnlyDictionary<string, T>, IEnumerable, IEnumerable<string>
    {
        private readonly Dictionary<string, T> _storage;

        internal IDStorage(Dictionary<string, T> dictionary = null) { _storage = dictionary ?? []; }

        public T this[string key] => _storage[key];

        public IEnumerable<string> Keys => _storage.Keys;

        public IEnumerable<T> Values => _storage.Values;

        public int Count => _storage.Count;

        public bool ContainsKey(string key) => _storage.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _storage.GetEnumerator();

        public bool TryGetValue(string key, out T value) => _storage.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _storage.GetEnumerator();

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => _storage.Keys.GetEnumerator();
    }
}
