using Amrv.ConfigurableCompany.API;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.model
{
    [Obsolete("Use CCategory")]
    public sealed class ConfigurationCategory
    {
        private static readonly Dictionary<string, ConfigurationCategory> _categories = new();

        private static ConfigurationCategory _defaultCategory;

        public static ConfigurationCategory Default
        {
            get
            {
                _defaultCategory ??= new ConfigurationCategoryBuilder()
                    .SetID("configurable_company_default_category")
                    .SetName("General")
                    .SetPage(ConfigurationPage.Default)
                    .HideIfEmpty(true)
                    .Build();

                return _defaultCategory;
            }
        }

        public static bool TryGet(string id, out ConfigurationCategory value) => _categories.TryGetValue(id, out value);
        public static bool Contains(string id) => _categories.ContainsKey(id);

        public static ConfigurationCategory Get(string id) => _categories[id];
        public static IReadOnlyDictionary<string, ConfigurationCategory> GetAll() => _categories;

        public static Dictionary<string, ConfigurationCategory>.ValueCollection Categories => _categories.Values;
        public static Dictionary<string, ConfigurationCategory>.KeyCollection Ids => _categories.Keys;

        internal readonly CCategory CCategory;

        public string ID => CCategory.ID;
        public string Name => CCategory.Name;
        public Color Color => CCategory.Color;
        public bool HideIfEmpty = true;
        public readonly ConfigurationPage Page;

        internal ConfigurationCategory(ConfigurationCategoryBuilder builder)
        {
            CCategory = builder.CBuilder;

            Page = builder.Page;

            _categories[ID] = this;
        }

        public override bool Equals(object obj)
        {
            return obj is ConfigurationCategory Category && Category.ID.Equals(ID);
        }

        public override int GetHashCode() => CCategory.GetHashCode() - 10;
    }
}
