using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.Net;
using Amrv.ConfigurableCompany.Plugin;

namespace Amrv.ConfigurableCompany.Core.Config
{
    internal static class ConfigEventRouter
    {
        public static void OnCreate_Page(CPage page)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Page ({page.ID})");
            MenuController.AddPage(page);
            //MenuController.SetCurrentPage(page);
            CEvents.ConfigEvents.CreatePage.Invoke(new(page));
        }

        public static void OnCreate_Category(CCategory category)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Category ({category.ID})");
            MenuController.AddCategory(category);
            CEvents.ConfigEvents.CreateCategory.Invoke(new(category));
        }

        public static void OnCreate_Section(CSection section)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Section ({section.ID})");
            MenuController.AddSection(section);
            CEvents.ConfigEvents.CreateSection.Invoke(new(section));
        }

        public static void OnCreate_Config(CConfig config)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Config ({config.ID})");
            MenuController.AddConfig(config);
            CEvents.ConfigEvents.CreateConfig.Invoke(new(config));
        }

        public static void OnChange_Config(CConfig config, ChangeReason reason, object oldValue, object requestedValue, bool succeded, bool converted)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnChange | Config ({config.ID}, {(succeded ? "Accepted" : "Denied")})");
            MenuController.UpdateConfig(config, reason);
            CEvents.ConfigEvents.ChangeConfig.Invoke(new(config, reason, oldValue, requestedValue, succeded, converted));
            if (succeded && NetSynchronizer.IsServer)
                NetController.SendConfig(config);
        }

        public static void OnToggle_Config(CConfig config, bool enabled)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnToggle | Config ({config.ID}, {(enabled ? "Enabled" : "Disabled")})");
            MenuController.TriggerToggleConfig(config, enabled);
            CEvents.ConfigEvents.ToggleConfig.Invoke(new(config, enabled));
        }
    }
}
