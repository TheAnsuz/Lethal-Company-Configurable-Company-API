using System.Collections;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.content.model
{
    public abstract class ConfigurationBundle : IEnumerable<KeyValuePair<string, string>>
    {
        public ConfigurationBundle() { }

        public abstract void Clear();

        public abstract void AddIfMissing(string key, string value);

        public abstract void Add(string key, string value);

        public abstract void Remove(string key);

        public abstract string Get(string key, string defaultValue);

        public abstract bool TryGet(string key, out string value);

        public abstract string Pack();

        public abstract void Unpack(string data);

        public abstract IEnumerator<KeyValuePair<string, string>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
