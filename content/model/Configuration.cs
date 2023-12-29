using Amrv.ConfigurableCompany.content.model.data;
using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.content.model
{
    public class Configuration
    {
        private static readonly Dictionary<string, Configuration> _configurations = new();

        public static bool TryGet(string id, out Configuration value) => _configurations.TryGetValue(id, out value);

        public static bool Contains(string id) => _configurations.ContainsKey(id);

        public static Configuration Get(string id) => _configurations[id];

        public static IReadOnlyDictionary<string, Configuration> GetAll() => _configurations;

        public static Dictionary<string, Configuration>.ValueCollection Configs => _configurations.Values;
        public static Dictionary<string, Configuration>.KeyCollection Ids => _configurations.Keys;

        public readonly string ID;
        public readonly ConfigurationType Type;
        public readonly string Tooltip;
        public readonly bool HasTooltip;
        public object Value { get; private set; }
        public readonly object Default;
        public readonly string Name;
        public readonly bool Synchronized;
        public readonly bool Experimental;
        public readonly ConfigurationCategory Category;

        internal protected Configuration(ConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentException($"Tried to create configuration without ConfigurationBuilder");

            if (builder.ID == null)
                throw new ArgumentException($"Tried to create configuration without ID");

            if (Contains(builder.ID))
                throw new ArgumentException($"The ID {builder.ID} for the configuration already exists");

            if (!DataUtils.IsValidID(builder.ID))
                throw new ArgumentException($"The ID {builder.ID} does not have a valid format");

            ID = builder.ID;

            if (builder.Name == null || builder.Name.Replace(" ", "").Length < 1)
                throw new ArgumentException($"Tried to create config {ID} without a name");

            Name = builder.Name;

            if (builder.Type == null)
                throw new ArgumentException($"Tried to create config {ID} without a ConfigurationType");

            Category = builder.Category;

            if (Category == null)
                throw new ArgumentException($"Tried to create config {ID} without a Category");

            Tooltip = FixTooltip(builder.Tooltip);

            HasTooltip = CheckIfHasTooltip(Tooltip);

            Synchronized = builder.Synchronized;

            Experimental = builder.Experimental;

            Type = builder.Type;

            if (!TrySet(builder.Value, ChangeReason.CONFIGURATION_CREATED))
            {
                if (!TrySet(Type.DefaultValue, ChangeReason.CONFIGURATION_CREATED))
                    throw new ArgumentException($"Tried to create config {ID} with an invalid value {builder.Value}/{Type.DefaultValue}");
            }
            Default = Value;

            _configurations.Add(ID, this);
        }

        protected virtual string FixTooltip(string tooltip)
        {
            return tooltip;
        }

        protected virtual bool CheckIfHasTooltip(string tooltip)
        {
            return tooltip != null && tooltip.Replace(" ", "").Length > 0;
        }

        public virtual bool TrySet(object value, ChangeReason reason = ChangeReason.SCRIPTED)
        {
            object old = Value;

            if (Type.IsValidValue(value))
            {
                Value = value;
                Events.ConfigurationChanged.Invoke(new(this, old, Value, reason, ChangeResult.SUCCESS));
                return true;
            }

            if (Type.TryConvert(value, out object result))
            {
                Value = result;
                Events.ConfigurationChanged.Invoke(new(this, old, Value, reason, ChangeResult.SUCCESS_CONVERTED));
                return true;
            }

            Events.ConfigurationChanged.Invoke(new(this, old, Value, reason, ChangeResult.FAILED));
            return false;
        }

        public virtual T Get<T>(T defaultValue = default) => TryGet(out T val) ? val : defaultValue;

        public virtual bool TryGet<T>(out T result)
        {
            if (Value is T converted)
            {
                result = converted;
                return true;
            }

            result = default;
            return false;
        }

        internal void Reset(ChangeReason reason)
        {
            object old = Value;
            Value = Default;
            Events.ConfigurationChanged.Invoke(new(this, old, Value, reason, ChangeResult.SUCCESS));
        }

        public virtual void Reset()
        {
            Reset(ChangeReason.SCRIPTED_RESET);
        }

        public override bool Equals(object obj)
        {
            if (obj is Configuration Config)
                return Config.ID.Equals(ID);
            return false;
        }

        public override int GetHashCode()
        {
            return 1213502048 + ID.GetHashCode();
        }
    }
}
