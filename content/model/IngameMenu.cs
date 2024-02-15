using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.model
{
    internal class IngameMenu
    {
        public const string ChallengeFileName = "LCChallengeFile";

        public static ConfigurationScreen ConfigDisplay { get; protected set; }
        public static bool ConfigDisplayExists => ConfigDisplay != null;

        public static bool ShouldIgnoreFile(/*SaveFileUISlot slot*/)
        {
            return GameNetworkManager.Instance?.saveFileNum == -1;
        }
        public static bool ShouldIgnoreFile(SaveFileUISlot slot)
        {
            return slot?.fileNum == -1;
        }
        public static bool ShouldIgnoreFile(int fileNum)
        {
            return fileNum == -1;
        }

        public static bool ShouldCreateMenu() => Configuration.Configs.Count > 0;

        public static void SafeInstantiate(MenuManager menuManager)
        {
            if (!ShouldCreateMenu())
                return;

            Events.BeforeMenuDisplay.Invoke(EventArgs.Empty);

            //SaveFileUISlot saveFile = UnityEngine.Object.FindObjectOfType<SaveFileUISlot>();

            if (!ShouldIgnoreFile(/*saveFile*/))
            {
                ConfigurationIO.ReadAll(FileUtils.GetCurrentConfigFileName());
            }

            if (ConfigDisplay == null || !ConfigDisplay.IsValid())
            {
                ConfigDisplay?.Destroy();

                CategoryIO.ReadAll();
                DisplayUtils.LoadFontAssets();

                ConfigDisplay = new ConfigurationScreen(menuManager.HostSettingsScreen);

                GameObject backButton = menuManager.HostSettingsScreen.transform.Find("HostSettingsContainer/Back")?.gameObject ?? null;
                Button backButtonComp = backButton?.GetComponent<Button>() ?? null;

                if (backButtonComp == null)
                {
                    ConfigurableCompanyPlugin.Error($"Can't get back button to add exit listener");
                }
                else
                {
                    backButtonComp.onClick.AddListener(delegate { SaveCurrentConfig(); SaveCategories(); });
                }
            }

            /*
            if (saveFile == null)
                ConfigDisplay.SetVisible(true);
            else
                ConfigDisplay.SetVisible(saveFile.fileNum != -1);
            */
            ConfigDisplay.SetVisible(!ShouldIgnoreFile());

            ConfigDisplay.Refresh();
            ConfigDisplay.LoadAll();

            Events.AfterMenuDisplay.Invoke(EventArgs.Empty);
        }

        public static void Delete()
        {
            ConfigDisplay = null;
        }

        public static void SetVisible(bool visible)
        {
            ConfigDisplay.SetVisible(visible);
        }

        public static void SaveCategories()
        {
            CategoryIO.SaveAll();
        }

        public static void LoadCategories()
        {
            CategoryIO.ReadAll();
        }

        public static void SaveCurrentConfig(string filename = null)
        {
            if (ConfigDisplay == null) return;

            ConfigDisplay.SaveAll();
            ConfigurationIO.SaveAll(filename ?? FileUtils.GetCurrentConfigFileName());
        }

        public static void LoadCurrentConfig(string filename = null)
        {
            if (ConfigDisplay == null) return;

            ConfigurationIO.ReadAll(filename ?? FileUtils.GetCurrentConfigFileName());
            ConfigDisplay.LoadAll();
        }

        public static void ResetCurrentConfig(string filename = null)
        {
            foreach (var config in Configuration.Configs)
                config.Reset(data.ChangeReason.USER_RESET);

            ConfigDisplay.LoadAll();
            ConfigurationIO.RemoveFile(filename ?? FileUtils.GetCurrentConfigFileName());
        }

        private IngameMenu() { }
    }
}
