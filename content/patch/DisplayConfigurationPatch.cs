using BepInEx.Configuration;
using Amrv.ConfigurableCompany.content.utils;
using HarmonyLib;
using System;
using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.model;

namespace Amrv.ConfigurableCompany.content.patch
{
    internal class DisplayConfigurationPatch
    {
        private static ConfigurationScreen _configDisplay = null;
        public static ConfigurationScreen ConfigDisplay => _configDisplay;

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ClickHostButton))]
        [HarmonyPostfix]
        private static void HostButtonClick(ref MenuManager __instance)
        {
            if (Configuration.Configs.Count == 0)
                return;

            ConfigurationIO.ReadAll(FileUtils.GetCurrentConfigFileName());

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
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay = null;
        }
    }
}
