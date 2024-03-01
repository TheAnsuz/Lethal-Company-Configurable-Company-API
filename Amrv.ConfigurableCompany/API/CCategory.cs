using Amrv.ConfigurableCompany.Core;
using Amrv.ConfigurableCompany.Core.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CCategory
    {
        private static CCategory _defaultCategory;

        public static CCategory Default
        {
            get
            {
                _defaultCategory ??= new CCategoryBuilder()
                {
                    ID = "configurable-company_category_default",
                    Color = new Color32(214, 90, 24, 255),
                    HideIfEmpty = true,
                    Name = "Configurations",
                    Page = CPage.Default
                };

                return _defaultCategory;
            }
        }

        private static readonly Dictionary<string, CCategory> _categories = [];

        /// <summary>
        /// A dictionary-like storage containing all the generated categories
        /// </summary>
        public static readonly IDStorage<CCategory> Storage = new(_categories);

        /// <summary>
        /// Get the standard builder to generate a new category
        /// </summary>
        /// <returns>The builder for a new category</returns>
        public static CCategoryBuilder Builder() => new();

        private readonly Dictionary<string, CSection> _sections;

        /// <summary>
        /// A dictionary-like storage containing all the sections attached to this category
        /// </summary>
        public readonly IDStorage<CSection> Sections;

        internal void AddSection(CSection section) => _sections.Add(section.ID, section);
        internal void RemoveSection(CSection section) => _sections.Remove(section.ID);

        private readonly Dictionary<string, CConfig> _configs;

        /// <summary>
        /// A dictionary-like storage containing all the configurations attached to this category
        /// </summary>
        public readonly IDStorage<CConfig> Configs;

        internal void AddConfig(CConfig config) => _configs.Add(config.ID, config);
        internal void RemoveConfig(CConfig config) => _configs.Remove(config.ID);

        /// <summary>
        /// The page this category is attached to
        /// </summary>
        public readonly CPage Page;

        /// <summary>
        /// The internal ID for this category
        /// </summary>
        public readonly string ID;
        /// <summary>
        /// The display name for this category
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The background color shown with the category's header display
        /// </summary>
        public readonly Color Color;
        /// <summary>
        /// If true, the category won't be shown in the menu if it does not have items
        /// </summary>
        public readonly bool HideIfEmpty;

        internal CCategory(CCategoryBuilder builder)
        {
            // Checks
            if (builder == null) throw new BuildingException("Tried to create category without builder");

            if (builder.ID == null) throw new BuildingException("Unable to build category without ID");

            if (_categories.ContainsKey(builder.ID)) throw new BuildingException($"Tried to create a category with an existing ID ({builder.ID})");

            if (builder.Page == null || !CPage.Storage.TryGetValue(builder.Page, out Page))
                throw new BuildingException($"Tried to create category within an undefined page {builder.Page}");

            // Sets
            ID = builder.ID;
            Name = builder.Name ?? "";
            Color = builder.Color;

            // Indexing
            _sections = [];
            Sections = new(_sections);
            _configs = [];
            Configs = new(_configs);

            // Index itslef
            _categories.Add(ID, this);
            Page.AddCategory(this);

            // Notify
            ConfigEventRouter.OnCreate_Category(this);
        }

        public override bool Equals(object obj)
        {
            return obj is CCategory other && other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            return 987361543 | ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"Section[{ID}:{Name}]";
        }
    }
}
