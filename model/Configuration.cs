using ConfigurableCompany.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model
{
    public sealed class Configuration
    {
        public static Dictionary<string, Configuration>.ValueCollection Registered => _registry.Values;

        private static readonly Dictionary<string, Configuration> _registry = new();

        public static Configuration Create(string id, string name, ConfigurationCategory category, ConfigurationType type, object value, string tooltip = null, bool syncronized = false, Events.OnConfigurationChange changeEvent = null)
        {
            // Validate and stuff
            if (!DataUtils.IsValidID(id))
                throw new ArgumentException($"Configuration id '{id}' is not valid");

            if (IsRegistered(id))
                throw new ArgumentException($"There is an existing configuration with id '{id}'");

            if (category == null) throw new ArgumentException($"Configuration '{id}' has a NULL category");

            if (type == null) throw new ArgumentException($"Configuration '{id}' has a NULL type");

            value ??= type.Default();

            if (!type.IsValid(value) && !type.TryConvert(value, out value))
                throw new ArgumentException($"Unable to create config {id} with default value {value}");

            Configuration configuration = new(id, name, category, type, value, tooltip, syncronized, changeEvent);
            category.Add(configuration);
            _registry.Add(id, configuration);

            Console.WriteLine($"Registered config {configuration.ID}");

            return configuration;
        }

        public static bool TryGet(string id, out Configuration config) => _registry.TryGetValue(id, out config);

        public static bool IsRegistered(string id) => _registry.ContainsKey(id);

        public readonly string ID;
        public readonly string Name;
        public readonly ConfigurationCategory Category;
        public readonly ConfigurationType Type;
        private object _value;
        private readonly object _defaultValue;
        public readonly string Tooltip;
        public readonly bool Syncronized;
        public readonly Events.OnConfigurationChange ChangeEvent;

        private Configuration(string id, string name, ConfigurationCategory category, ConfigurationType type, object value, string tooltip, bool syncronized, Events.OnConfigurationChange changeEvent)
        {
            ID = id;
            Name = name;
            Category = category;
            Type = type;
            _value = value;
            _defaultValue = value;
            Tooltip = tooltip;
            Syncronized = syncronized;
        }

        public object GetRaw() => _value;

        public T Get<T>()
        {
            //Console.WriteLine($"Trying to get value {_value} from {ID} of type{Type.Name} as {typeof(T)}");

            if (_value is T)
                return (T)_value;

            if (Type.TryConvert(_value, out object output))
                return (T)output;

            return (T)_defaultValue;
        }

        public T Get<T>(T defaultValue)
        {
            if (_value is T converted)
                return converted;

            //Console.WriteLine($"{_value} is not of type {typeof(T)}");

            if (Type.TryConvert(_value, out object output))
                return (T)output;

            //Console.WriteLine($"{_value} cant be converted to {typeof(T)}, using default {defaultValue}");

            return defaultValue;
        }

        public void Reset(ChangeReason reason)
        {
            ConfigurableCompanyPlugin.Debug($"[Configuration::Reset] {ID} reset to {_defaultValue}");
            Set(_defaultValue, reason);
        }

        public bool Set(object value, ChangeReason reason)
        {
            if (ChangeEvent != null)
                ChangeEvent.Invoke(reason, _value, ref value);

            if (Type.IsValid(value))
            {
                ConfigurableCompanyPlugin.Debug($"[Configuration::Set] {ID} set to {value} (replaced)");
                _value = value;
                return true;
            }

            if (Type.TryConvert(value, out _value))
            {
                ConfigurableCompanyPlugin.Debug($"[Configuration::Set] {ID} set to {value} (converted)");
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Configuration config)
                return ID.Equals(config.ID);

            return false;
        }

        public override int GetHashCode()
        {
            return 1213502049 + ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{ID}:{_value} {Type.Name} {(Syncronized ? "Sync" : "Async")}]";
        }
    }
}
