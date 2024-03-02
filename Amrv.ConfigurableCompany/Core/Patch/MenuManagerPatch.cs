using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using HarmonyLib;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Patch
{
    [HarmonyPatch(typeof(MenuManager))]
    internal class MenuManagerPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Start_Postfix(MenuManager __instance)
        {
            ConfigurableCompanyPlugin.Debug($"MenuManager::Start [Postfix]");
            if (!__instance.isInitScene)
            {
                //CategoryIO.Load();
                LifecycleEventRouter.CreateMenu();
                __instance.HostSettingsScreen.FindChild("HostSettingsContainer/Back").GetComponent<Button>().onClick.AddListener(ClickBackButton_Event);
            }

        }

        private static void ClickBackButton_Event()
        {
            MenuController.SetVisible(false);
        }

        [HarmonyPatch(nameof(MenuManager.ClickHostButton))]
        [HarmonyPostfix]
        private static void ClickHostButton_Postfix()
        {
            MenuController.SetVisible(true);
        }

        [HarmonyPatch(nameof(MenuManager.ConfirmHostButton))]
        [HarmonyPostfix]
        private static void ConfirmHostButton_Postfix()
        {
            LifecycleEventRouter.DestroyMenu();
        }
    }
}
