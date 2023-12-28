using Amrv.ConfigurableCompany.content.model;
using System;
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
    }
}
