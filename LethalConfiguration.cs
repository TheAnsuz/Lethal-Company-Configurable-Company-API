using Amrv.ConfigurableCompany.content.model;

namespace Amrv.ConfigurableCompany
{
    public sealed class LethalConfiguration
    {
        private LethalConfiguration() { }

        public const string PLUGIN_GUID = ConfigurableCompanyPlugin.PLUGIN_GUID;
        public const string PLUGIN_VERSION = ConfigurableCompanyPlugin.PLUGIN_VERSION;

        public static ConfigurationBuilder CreateConfig() => new();
        public static ConfigurationBuilder CreateConfig(string id) => new(id);

        public static ConfigurationCategoryBuilder CreateCategory() => new();
        public static ConfigurationCategoryBuilder CreateCategory(string id) => new(id);
    }
}
