using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class ConfigurationCategory
    {
        private static readonly Dictionary<string, ConfigurationCategory> _categories = new();

        private static readonly ConfigurationCategory _defaultCategory = LethalConfiguration.CreateCategory()
            .SetID("configurable_company_default_category")
            .SetName("General")
            .SetPage(ConfigurationPage.Default)
            .HideIfEmpty(true)
            .Build();

        public static ConfigurationCategory Default => _defaultCategory;

        public static bool TryGet(string id, out ConfigurationCategory value) => _categories.TryGetValue(id, out value);
        public static bool Contains(string id) => _categories.ContainsKey(id);

        public static ConfigurationCategory Get(string id) => _categories[id];
        public static IReadOnlyDictionary<string, ConfigurationCategory> GetAll() => _categories;

        public static Dictionary<string, ConfigurationCategory>.ValueCollection Categories => _categories.Values;
        public static Dictionary<string, ConfigurationCategory>.KeyCollection Ids => _categories.Keys;

        public readonly string ID;
        public readonly string Name;
        public readonly Color Color;
        public readonly bool HideIfEmpty;
        public readonly ConfigurationPage Page;

        internal ConfigurationCategory(ConfigurationCategoryBuilder builder)
        {
            if (builder == null)
                throw new ArgumentException($"Tried to create category without ConfigurationCategoryBuilder");

            if (builder.ID == null)
                throw new ArgumentException($"Tried to create category without ID");

            if (Contains(builder.ID))
                throw new ArgumentException($"The ID {builder.ID} for the category already exists");

            if (!DataUtils.IsValidID(builder.ID))
                throw new ArgumentException($"The ID {builder.ID} does not have a valid format");

            ID = builder.ID;

            if (builder.Name == null)
                throw new ArgumentException($"Tried to create category {ID} without a name");

            if (builder.Page == null)
                throw new ArgumentException($"Tried to create category {ID} without a page");

            HideIfEmpty = builder.HidesIfEmpty;

            Color = builder.Color;

            Name = builder.Name;

            Page = builder.Page;

            _categories[ID] = this;
        }

        public override bool Equals(object obj)
        {
            if (obj is ConfigurationCategory Category)
            {
                return Category.ID.Equals(ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 65836853 + ID.GetHashCode();
        }
    }
}
