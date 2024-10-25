using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.IO;
using System.Collections;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core
{
    internal static class LifecycleEventRouter
    {
        public static void PluginStart()
        {
            CEvents.LifecycleEvents.PluginStart.Invoke();
        }

        public static void CreateMenu(MenuManager manager)
        {
            MenuLoader.Create();
            IOController.LoadCategories();
            IOController.LoadSections();
            IOController.LoadConfigs();
            IOController.GetConfigCache();

            MenuController.AfterCreation(AfterCreation);

            foreach (var canvas in Object.FindObjectsOfType<Canvas>())
            {
                if (canvas.gameObject.transform.parent == null && canvas.gameObject.scene.name == "MainMenu")
                {
                    MenuController.Create(canvas.gameObject, manager);
                    break;
                }
            }

        }

        private static IEnumerator AfterCreation(MenuManager manager)
        {
            MenuController.SetLocked(GameNetworkManager.Instance?.currentSaveFileName == "LCChallengeFile");
            MenuController.UpdateSeedFromCache();
            yield return null;
            MenuLoader.Destroy();
            yield return null;
        }

        public static void DestroyMenu()
        {
            IOController.SaveCategories();
            IOController.SaveSections();
            IOController.SaveConfigs();
            MenuController.Destroy();
            MenuLoader.Destroy();
        }
    }
}
