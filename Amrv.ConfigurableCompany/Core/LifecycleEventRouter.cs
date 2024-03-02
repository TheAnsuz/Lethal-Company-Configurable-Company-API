using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Plugin;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core
{
    internal static class LifecycleEventRouter
    {
        public static void PluginStart()
        {
            CEvents.LifecycleEvents.PluginStart.Invoke();
        }

        public static void CreateMenu()
        {
            ConfigurableCompanyPlugin.Debug($"Creating menu");
            IOController.LoadCategories();
            IOController.LoadConfigs();
            IOController.GetConfigCache();
            foreach (var canvas in Object.FindObjectsOfType<Canvas>())
            {
                if (canvas.gameObject.transform.parent == null && canvas.gameObject.scene.name == "MainMenu")
                {
                    MenuController.Create(canvas.gameObject);
                    break;
                }
            }

            MenuController.SetLocked(GameNetworkManager.Instance?.currentSaveFileName == "LCChallengeFile");
        }

        public static void DestroyMenu()
        {
            IOController.SaveCategories();
            IOController.SaveConfigs();
            MenuController.Destroy();
        }
    }
}
