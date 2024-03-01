using Amrv.ConfigurableCompany.Core;
using Amrv.ConfigurableCompany.Core.Config;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CSection
    {
        private static readonly Dictionary<string, CSection> _sections = [];
        public static readonly IDStorage<CSection> Storage = new(_sections);

        public static CSectionBuilder Builder() => new();

        private readonly Dictionary<string, CConfig> _configs;
        public readonly IDStorage<CConfig> Configs;

        internal void AddConfig(CConfig config) => _configs.Add(config.ID, config);
        internal void RemoveConfig(CConfig config) => _configs.Remove(config.ID);

        public readonly CCategory Category;

        public readonly string ID;
        public readonly string Name;

        internal CSection(CSectionBuilder builder)
        {
            // Checks
            if (builder == null) throw new BuildingException("Tried to create section without builder");

            if (builder.ID == null) throw new BuildingException("Unable to build section without ID");

            if (_sections.ContainsKey(builder.ID)) throw new BuildingException($"Tried to create a section with an existing ID ({builder.ID})");

            if (builder.Category == null || !CCategory.Storage.TryGetValue(builder.Category, out Category))
                throw new BuildingException($"Tried to create category within undefined page {builder.Category}");

            // Sets
            ID = builder.ID;
            Name = builder.Name ?? "";

            // Indexing
            _configs = [];
            Configs = new(_configs);

            // Index itself
            _sections.Add(ID, this);
            Category.AddSection(this);

            // Notify
            ConfigEventRouter.OnCreate_Section(this);
        }

        public override bool Equals(object obj)
        {
            return obj is CSection other && other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            return 1902536834 | ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"Section[{ID}:{Name}]";
        }
    }
}
