using Amrv.ConfigurableCompany.content.model;
using System.Collections.Generic;
using System.Reflection;

namespace Amrv.ConfigurableCompany
{
    public sealed class LethalConfiguration
    {
        private LethalConfiguration() { }

        public const string PLUGIN_GUID = ConfigurableCompanyPlugin.PLUGIN_GUID;
        public const string PLUGIN_VERSION = ConfigurableCompanyPlugin.PLUGIN_VERSION;

        public static ConfigurationBuilder CreateConfig()
        {
            AssemblyRegistry.RegisterAssembly(Assembly.GetCallingAssembly());
            return new();
        }
        public static ConfigurationBuilder CreateConfig(string id)
        {
            AssemblyRegistry.RegisterAssembly(Assembly.GetCallingAssembly());
            return new(id);
        }

        public static ConfigurationCategoryBuilder CreateCategory()
        {
            AssemblyRegistry.RegisterAssembly(Assembly.GetCallingAssembly());
            return new();
        }
        public static ConfigurationCategoryBuilder CreateCategory(string id)
        {
            AssemblyRegistry.RegisterAssembly(Assembly.GetCallingAssembly());
            return new(id);
        }

        public static bool TryGetConfig(string id, out Configuration config) => Configuration.TryGet(id, out config);

        public static Dictionary<string, Configuration>.KeyCollection ConfigIDs => Configuration.Ids;
        public static Dictionary<string, Configuration>.ValueCollection Configs => Configuration.Configs;

        public static bool TryGetCategory(string id, out ConfigurationCategory category) => ConfigurationCategory.TryGet(id, out category);
        public static Dictionary<string, ConfigurationCategory>.KeyCollection CategoryIDs => ConfigurationCategory.Ids;
        public static Dictionary<string, ConfigurationCategory>.ValueCollection Categories => ConfigurationCategory.Categories;
    }
}
