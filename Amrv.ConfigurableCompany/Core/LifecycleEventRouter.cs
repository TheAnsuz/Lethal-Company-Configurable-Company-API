using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.IO;
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
            IOController.LoadCategories();
            IOController.LoadConfigs();
            IOController.GetConfigCache();
            MenuController.Create(GameObject.Find("Canvas"));

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
