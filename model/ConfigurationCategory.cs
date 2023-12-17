using ConfigurableCompany.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConfigurableCompany.model
{
    public sealed class ConfigurationCategory
    {
        public static Dictionary<string, ConfigurationCategory>.ValueCollection Registered => _registry.Values;
        public static readonly ConfigurationCategory DEFAULT = new("default_category", "General");

        private static readonly Dictionary<string, ConfigurationCategory> _registry = new();

        public static ConfigurationCategory Create(string id, string displayName)
        {
            if (!DataUtils.IsValidID(id))
                throw new ArgumentException($"Configuration category id '{id}' is not valid");

            if (IsRegistered(id))
                throw new ArgumentException($"There is an existing category with id '{id}'");

            if (displayName == null)
                throw new ArgumentException($"Category '{id}' has a NULL display name");

            // Validate and stuff
            ConfigurationCategory category = new(id, displayName);
            _registry.Add(id, category);

            Console.WriteLine($"Registered category {category.ID}");

            return category;
        }

        public static bool TryGet(string id, out ConfigurationCategory category) => _registry.TryGetValue(id, out category);

        public static bool IsRegistered(string id) => _registry.ContainsKey(id);

        public ReadOnlyCollection<Configuration> Configurations => _configs.AsReadOnly();

        internal void Add(Configuration config) => _configs.Add(config);

        private readonly List<Configuration> _configs = new();

        public readonly string ID;
        public readonly string Name;

        private ConfigurationCategory(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is ConfigurationCategory category)
                return ID.Equals(category.ID);
            return false;
        }

        public override int GetHashCode()
        {
            return 1969571243 + ID.GetHashCode();
        }

        public override string ToString()
        {
            return ID;
        }
    }
}
