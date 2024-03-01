using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Plugin;
using HarmonyLib;

namespace Amrv.ConfigurableCompany.Core.Patch
{
    [HarmonyPatch(typeof(SaveFileUISlot))]
    internal class SaveFileUISlotPatch
    {
        public static bool Patch_SetFileToThis_Postfix = true;

        [HarmonyPatch(nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        static void SetFileToThis_Postfix(SaveFileUISlot __instance)
        {
            if (!Patch_SetFileToThis_Postfix)
                return;

            ConfigurableCompanyPlugin.Debug($"SaveFileUISlot::SetFileToThis [Postfix] | fileNum: {__instance.fileNum} |");

            MenuController.SetLocked(__instance.fileNum == -1);

            IOController.LoadConfigs();
            IOController.GetConfigCache();
            MenuController.SetCurrentFileName(GameNetworkManager.Instance.currentSaveFileName);
        }

    }
}
