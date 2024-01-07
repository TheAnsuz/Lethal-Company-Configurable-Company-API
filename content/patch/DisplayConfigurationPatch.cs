using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.utils;
using HarmonyLib;
using System;

namespace Amrv.ConfigurableCompany.content.patch
{
    internal class DisplayConfigurationPatch
    {
        private static ConfigurationScreen _configDisplay = null;
        public static ConfigurationScreen ConfigDisplay => _configDisplay;
        public static bool ConfigDisplayExists => _configDisplay != null;

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ClickHostButton))]
        [HarmonyPostfix]
        private static void HostButtonClick(ref MenuManager __instance)
        {
            Events.BeforeMenuDisplay.Invoke(EventArgs.Empty);

            if (Configuration.Configs.Count == 0)
                return;

            ConfigurationIO.ReadAll(FileUtils.GetCurrentConfigFileName());
#if DEBUG
            Console.WriteLine("DisplayConfigurationPatch::HostButtonClick");
#endif  

            if (ConfigDisplay == null)
            {
#if DEBUG
                Console.WriteLine("Creating menu");
#endif  
                DisplayUtils.LoadFontAssets();
                _configDisplay = new ConfigurationScreen(__instance.HostSettingsScreen);
            }

            _configDisplay.Refresh();
            _configDisplay.LoadAll();

            Events.AfterMenuDisplay.Invoke(EventArgs.Empty);
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPrefix]
        private static void ChangeSelectedFile_Pre()
        {
            //_configDisplay.SaveAllToConfig();
            _configDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void ChangeSelectedFile_Post()
        {
            //Plugin.Instance.LoggerObj().LogWarning("Change selected file
            ConfigurationIO.ReadAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay.LoadAll();
            //_configDisplay.LoadAllFromConfig();
        }

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ConfirmHostButton))]
        [HarmonyPostfix]
        private static void ConfirmHostButton_Post()
        {
            //Plugin.Instance.LoggerObj().LogWarning("Confirm host");
            //_configDisplay.SaveAllToConfig();
            _configDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay = null;
        }

        [HarmonyPatch(typeof(MenuManager), "ClickQuitButton")]
        [HarmonyPrefix]
        private static void QuitButton_Prefix()
        {
            _configDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
        }
    }
}
