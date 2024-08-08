using Amrv.ConfigurableCompany.Core;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API.Data
{
    public class SpecialSeed
    {
        private static readonly Dictionary<string, SpecialSeed> _specialSeed = [];
        public static readonly IReadOnlyDictionary<string, SpecialSeed> Seeds = new ReadonlyDictionary<string, SpecialSeed>(_specialSeed);

        public static bool IsSpecialSeed(string seed, out SpecialSeed specialSeed) => Seeds.TryGetValue(seed, out specialSeed);

        public static SpecialSeed Define(string name, int seed) => new(name, seed);

        public readonly int Index;
        public readonly string Name;
        public readonly long Seed;
        private readonly Dictionary<string, object> _configurations;
        public readonly IReadOnlyDictionary<string, object> Configurations;

        public SpecialSeed(string name, bool useDefaultConfigs = true) : this(name, useDefaultConfigs ? 0 : RandomSeedParser.FromSeed(name)) { }

        public SpecialSeed(string name, long seed)
        {
            Name = name.ToUpper();
            Seed = seed;
            _configurations = [];
            Configurations = new ReadonlyDictionary<string, object>(_configurations);
            _specialSeed[name] = this;
        }

        public object this[CConfig config]
        {
            get => Get(config);
            set => Set(config, value);
        }

        public object this[string configId]
        {
            get => Get(configId);
            set => Set(configId, value);
        }

        public SpecialSeed Add(CConfig config, object presetValue)
        {
            Set(config, presetValue);
            return this;
        }

        public SpecialSeed Add(string configId, object presetValue)
        {
            Set(configId, presetValue);
            return this;
        }

        public void Set(CConfig config, object presetValue) => _configurations[config.ID] = presetValue;

        public void Set(string configId, object presetValue) => _configurations[configId] = presetValue;

        public bool TrySet(CConfig config, object presetValue)
        {
            if (!_configurations.ContainsKey(config.ID))
            {
                _configurations[config.ID] = presetValue;
                return true;
            }
            return false;
        }

        public bool TrySet(string configId, object presetValue)
        {
            if (!_configurations.ContainsKey(configId))
            {
                _configurations[configId] = presetValue;
                return true;
            }
            return false;
        }

        public object Get(CConfig config, object @default = null) => _configurations.GetValueOrDefault(config.ID, @default);

        public object Get(string configId, object @default = null) => _configurations.GetValueOrDefault(configId, @default);

        public bool TryGet(CConfig config, out object value) => _configurations.TryGetValue(config.ID, out value);

        public bool TryGet(string configId, out object value) => _configurations.TryGetValue(configId, out value);
    }
}
