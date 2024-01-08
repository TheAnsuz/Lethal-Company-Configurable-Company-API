using Amrv.ConfigurableCompany.content.model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Amrv.ConfigurableCompany
{
    public sealed class LethalConfiguration
    {
        private LethalConfiguration() { }

        private static readonly Dictionary<Assembly, ConfigurationPage> _defaultModPages = new();
        private static readonly Dictionary<Assembly, ConfigurationCategory> _defaultModCategories = new();
        public const string PLUGIN_GUID = ConfigurableCompanyPlugin.PLUGIN_GUID;
        public const string PLUGIN_VERSION = ConfigurableCompanyPlugin.PLUGIN_VERSION;

        public static ConfigurationCategory DefaultCategory => ConfigurationCategory.Default;
        public static ConfigurationPage DefaultPage => ConfigurationPage.Default;

        public static ConfigurationBuilder CreateConfig()
        {
            return new();
        }
        public static ConfigurationBuilder CreateConfig(string id)
        {
            return new(id);
        }

        public static ConfigurationCategoryBuilder CreateCategory()
        {
            return new();
        }
        public static ConfigurationCategoryBuilder CreateCategory(string id)
        {
            return new(id);
        }

        public static ConfigurationPageBuilder CreatePage()
        {
            return new();
        }

        public static ConfigurationPage PageForMyMod(string name = null)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            if (!_defaultModPages.TryGetValue(assembly, out ConfigurationPage page))
            {
                page = new ConfigurationPageBuilder()
                {
                    Name = name ?? Assembly.GetCallingAssembly().GetName().Name,
                }.Build();
                _defaultModPages.Add(assembly, page);
            }
            return page;
        }

        public static ConfigurationCategory CategoryForMyMod(string name = "General")
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            if (!_defaultModCategories.TryGetValue(assembly, out ConfigurationCategory category))
            {
                category = new ConfigurationCategoryBuilder()
                .SetID(assembly.GetName().Name.Replace(" ", "_").Replace(".", "-").ToLower())
                .SetName(name ?? Assembly.GetCallingAssembly().GetName().Name);

                _defaultModCategories.Add(assembly, category);
            }

            return category;
        }

        public static bool TryGetConfig(string id, out Configuration config) => Configuration.TryGet(id, out config);

        public static Dictionary<string, Configuration>.KeyCollection ConfigIDs => Configuration.Ids;
        public static Dictionary<string, Configuration>.ValueCollection Configs => Configuration.Configs;

        public static bool TryGetCategory(string id, out ConfigurationCategory category) => ConfigurationCategory.TryGet(id, out category);
        public static Dictionary<string, ConfigurationCategory>.KeyCollection CategoryIDs => ConfigurationCategory.Ids;
        public static Dictionary<string, ConfigurationCategory>.ValueCollection Categories => ConfigurationCategory.Categories;
    }
}
