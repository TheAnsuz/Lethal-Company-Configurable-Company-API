using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.Display.Menu;
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
                MenuLoader.Create();
                LifecycleEventRouter.CreateMenu(__instance);
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
            ConfigurableCompanyPlugin.Debug($"MenuManager::ClickHostButton [Postfix]");

            MenuController.SetVisible(true);
        }

        [HarmonyPatch(nameof(MenuManager.ConfirmHostButton))]
        [HarmonyPostfix]
        private static void ConfirmHostButton_Postfix()
        {
            ConfigurableCompanyPlugin.Debug($"MenuManager::ConfirmHostButton [Postfix]");

            MenuController.DestroyIfCreating();
            LifecycleEventRouter.DestroyMenu();
        }
    }
}
