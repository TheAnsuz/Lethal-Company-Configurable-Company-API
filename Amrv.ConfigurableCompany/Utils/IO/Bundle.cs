using System.Collections;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.Utils.IO
{
    public abstract class Bundle : IEnumerable
    {
        public abstract IEnumerable<string> Keys { get; }
        public abstract IEnumerable<string> Values { get; }
        public abstract int Count { get; }

        public string this[string key]
        {
            get => Get(key, null);
            set => Set(key, value);
        }

        public abstract void Clear();

        public abstract void Set(string key, string value);

        public abstract void Add(string key, string value);

        public abstract void Remove(string key);

        public abstract string Get(string key, string defaultValue);

        public abstract string Pack();
        public abstract void Unpack(string data);

        public abstract bool Contains(string key);
        public abstract bool TryGet(string key, out string value);

        public abstract IEnumerator<KeyValuePair<string, string>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
