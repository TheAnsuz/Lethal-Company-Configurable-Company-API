using BepInEx.Configuration;
using ConfigurableCompany.display;
using ConfigurableCompany.model;
using ConfigurableCompany.utils;
using HarmonyLib;
using System;

namespace ConfigurableCompany.patch
{
    internal class DisplayConfigurationPatch
    {
        public static ConfigurationDisplay ConfigDisplay => _configDisplay;

        private static ConfigurationDisplay _configDisplay = null;

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ClickHostButton))]
        [HarmonyPostfix]
        private static void HostButtonClick(ref MenuManager __instance)
        {
            if (Configuration.Registered.Count == 0)
                return;

            ConfigurationLoader.LoadAll(FileUtils.GetCurrentConfigFileName());

            if (ConfigDisplay == null)
            {
                Console.WriteLine("Creating menu");
                DisplayUtils.LoadFontAssets();
                DisplayUtils.RegisterTypes();
                _configDisplay = new ConfigurationDisplay(__instance.HostSettingsScreen);
            }
            else
            {
                _configDisplay.LoadAllFromConfig();
            }

        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPrefix]
        private static void Change_SelectedFile_PrePatch()
        {
            _configDisplay.SaveAllToConfig();
            ConfigurationLoader.SaveAll(FileUtils.GetCurrentConfigFileName());
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void ChangeSelectedFile_PostPatch()
        {
            //Plugin.Instance.LoggerObj().LogWarning("Change selected file
            ConfigurationLoader.LoadAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay.LoadAllFromConfig();
        }

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ConfirmHostButton))]
        [HarmonyPostfix]
        private static void ConfirmHostButton_Patch()
        {
            //Plugin.Instance.LoggerObj().LogWarning("Confirm host");
            _configDisplay.SaveAllToConfig();
            ConfigurationLoader.SaveAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay = null;
        }
    }
}
