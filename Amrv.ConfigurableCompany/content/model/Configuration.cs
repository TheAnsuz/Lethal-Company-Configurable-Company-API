using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.content.model.data;
using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.content.model
{
    [Obsolete("Use CConfig")]
    public class Configuration
    {
        private static readonly Dictionary<string, Configuration> _configurations = new();

        public static bool TryGet(string id, out Configuration value) => _configurations.TryGetValue(id, out value);

        public static bool Contains(string id) => _configurations.ContainsKey(id);

        public static Configuration Get(string id) => _configurations[id];

        public static IReadOnlyDictionary<string, Configuration> GetAll() => _configurations;

        public static Dictionary<string, Configuration>.ValueCollection Configs => _configurations.Values;
        public static Dictionary<string, Configuration>.KeyCollection Ids => _configurations.Keys;

        private readonly CConfig config;

        public string ID => config.ID;
        public readonly ConfigurationType Type;
        public string Tooltip => config.Tooltip;
        public bool HasTooltip => !string.IsNullOrEmpty(config.Tooltip);
        public object Value => config.Value;
        public object Default => config.Default;
        public string Name => config.Name;
        public bool Synchronized => config.Synchronized;
        public bool Experimental => config.Experimental;
        public readonly bool NeedsRestart = false;
        public readonly ConfigurationCategory Category;

        protected internal Configuration(ConfigurationBuilder builder)
        {
            if (builder.Type == null)
                throw new ArgumentException($"Tried to create config {ID} without a ConfigurationType");

            Type = builder.Type;
            Category = builder.Category;

            config = builder.CBuilder;

            _configurations.Add(ID, this);
        }

        protected virtual string FixTooltip(string tooltip) => tooltip;

        protected virtual bool CheckIfHasTooltip(string tooltip) => false;

        public virtual bool TrySet(object value, data.ChangeReason reason = data.ChangeReason.SCRIPTED, IFormatProvider formatter = null) => config.TrySet(value, reason.NewReason(), formatter);

        public virtual T Get<T>(T defaultValue = default) => config.Get(defaultValue);

        public virtual bool TryGet<T>(out T result) => config.TryGet(out result);

        internal void Reset(data.ChangeReason reason) => config.Reset(reason.NewReason());

        public string ValueToString()
        {
            config.SerializeValue(out string data);
            return data;
        }

        public virtual void Reset() => config.Reset();

        public override bool Equals(object obj) => obj is Configuration Config && Config.ID.Equals(ID);

        public override int GetHashCode() => config.GetHashCode() - 10;
    }
}
