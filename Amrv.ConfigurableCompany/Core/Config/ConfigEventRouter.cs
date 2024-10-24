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
#if EXTENDED_LOG
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Page ({page.ID})");
#endif
            MenuController.AddPage(page);
            //MenuController.SetCurrentPage(page);
            CEvents.ConfigEvents.CreatePage.Invoke(new(page));
        }

        public static void OnCreate_Category(CCategory category)
        {
#if EXTENDED_LOG
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Category ({category.ID})");
#endif
            MenuController.AddCategory(category);
            CEvents.ConfigEvents.CreateCategory.Invoke(new(category));
        }

        public static void OnCreate_Section(CSection section)
        {
#if EXTENDED_LOG
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Section ({section.ID})");
#endif
            MenuController.AddSection(section);
            CEvents.ConfigEvents.CreateSection.Invoke(new(section));
        }

        public static void OnCreate_Config(CConfig config)
        {
#if EXTENDED_LOG
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnCreate | Config ({config.ID})");
#endif
            MenuController.AddConfig(config);
            CEvents.ConfigEvents.CreateConfig.Invoke(new(config));
        }

        public static void OnChange_Config(CConfig config, ChangeReason reason, object oldValue, object requestedValue, bool succeded, bool converted)
        {
#if EXTENDED_LOG
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnChange | Config ({config.ID}, {(succeded ? "Accepted" : "Denied")})");
#endif
            MenuController.UpdateConfig(config, reason);
            CEventChangeConfig @event = new(config, reason, oldValue, requestedValue, succeded, converted);
            CEvents.ConfigEvents.ConfigChangeSingle[config]?.Invoke(@event);
            CEvents.ConfigEvents.ChangeConfig.Invoke(@event);
            if (succeded && NetSynchronizer.IsServer)
                NetController.SendConfig(config);
        }

        public static void OnToggle_Config(CConfig config, bool enabled)
        {
#if EXTENDED_LOG
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnToggle | Config ({config.ID}, {(enabled ? "Enabled" : "Disabled")})");
#endif
            MenuController.TriggerToggleConfig(config, enabled);
            CEvents.ConfigEvents.ToggleConfig.Invoke(new(config, enabled));
        }

        public static void OnPreset_Create(string name)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnPreset | Create ({name})");
            MenuController.RefreshPresets();
            CEvents.IOSEvents.PresetUpdate.Invoke(new(name, CEventUpdatePreset.PresetAction.CREATE));
        }

        public static void OnPreset_Delete(string name)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnPreset | Delete ({name})");
            MenuController.RefreshPresets();
            CEvents.IOSEvents.PresetUpdate.Invoke(new(name, CEventUpdatePreset.PresetAction.DELETE));
        }

        public static void OnPreset_Update(string name)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnPreset | Update ({name})");
            //MenuController.RefreshPresets();
            CEvents.IOSEvents.PresetUpdate.Invoke(new(name, CEventUpdatePreset.PresetAction.UPDATE));
        }

        public static void OnPreset_Stablish(string name)
        {
            ConfigurableCompanyPlugin.Debug($"ConfigEventRouter > OnPreset | Stablish ({name})");
            //MenuController.RefreshPresets();
            MenuController.LoadConfigs();
            CEvents.IOSEvents.PresetUpdate.Invoke(new(name, CEventUpdatePreset.PresetAction.STABLISH));
        }
    }
}
