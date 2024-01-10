using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.utils;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

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

                GameObject backButton = __instance.HostSettingsScreen.transform.Find("Panel/Back")?.gameObject ?? null;
                Button backButtonComp = backButton?.GetComponent<Button>() ?? null;

                if (backButtonComp == null)
                {
                    ConfigurableCompanyPlugin.Error($"Can't get back button to add exit listener");
                }
                else
                {
                    backButtonComp.onClick.AddListener(BackButton_Click);
                }
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
            if (ConfigDisplay == null)
                return;

            _configDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void ChangeSelectedFile_Post(SaveFileUISlot __instance)
        {
            if (ConfigDisplay == null)
                return;

            ConfigDisplay.SetVisible(__instance.fileNum != -1);

            //Plugin.Instance.LoggerObj().LogWarning("Change selected file
            ConfigurationIO.ReadAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay.LoadAll();
            //_configDisplay.LoadAllFromConfig();
        }

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ConfirmHostButton))]
        [HarmonyPostfix]
        private static void ConfirmHostButton_Post()
        {
            if (ConfigDisplay == null)
                return;

            //Plugin.Instance.LoggerObj().LogWarning("Confirm host");
            //_configDisplay.SaveAllToConfig();
            _configDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
            _configDisplay = null;
        }

        //[HarmonyPatch(typeof(MenuManager), "ClickQuitButton")]
        //[HarmonyPrefix]
        private static void QuitButton_Prefix()
        {
            if (ConfigDisplay == null)
                return;

            ConfigDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
        }

        private static void BackButton_Click()
        {
#if DEBUG
            Console.WriteLine($"ClickBackButton -> savedConfig");
#endif
            ConfigDisplay.SaveAll();
            ConfigurationIO.SaveAll(FileUtils.GetCurrentConfigFileName());
        }
    }
}
