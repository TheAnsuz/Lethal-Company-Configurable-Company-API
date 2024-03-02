using Amrv.ConfigurableCompany.Core.Display;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Core.Patch;
using Amrv.ConfigurableCompany.Plugin;
using HarmonyLib;

namespace Amrv.ConfigurableCompany.Core.Dependency
{
    public class BetterSavesDependency : CDependency
    {
        public const BepInEx.BepInDependency.DependencyFlags DependencyType = BepInEx.BepInDependency.DependencyFlags.SoftDependency;

        public const string GUID = "LCBetterSaves";

        public override string AssemblyName => "LCBetterSaves";

        public override void Process(Harmony harmony)
        {
            SaveFileUISlotPatch.Patch_SetFileToThis_Postfix = false;
            harmony.PatchAll(typeof(BetterSavesDependency));
        }

        [HarmonyPatch(typeof(SaveFileUISlot_BetterSaves))]
        [HarmonyPatch(nameof(SaveFileUISlot_BetterSaves.SetFileToThis))]
        [HarmonyPostfix]
        private static void SaveFileUISlot_BetterSaves_SetFileToThis_Postfix(SaveFileUISlot_BetterSaves __instance)
        {
            ConfigurableCompanyPlugin.Debug($"SaveFileUISlot_BetterSaves::SetFileToThis [Postfix] | fileNum: {__instance.fileNum} | fileString: {__instance.fileString} |");

            MenuController.SetLocked(__instance.fileNum == -1);

            if (__instance.fileNum == -1)
                IOController.RemoveConfigs();
            else
                IOController.LoadConfigs();

            IOController.GetConfigCache();
            MenuController.SetCurrentFileName(__instance.fileString);
        }

        [HarmonyPatch(typeof(NewFileUISlot_BetterSaves))]
        [HarmonyPatch(nameof(NewFileUISlot_BetterSaves.SetFileToThis))]
        [HarmonyPostfix]
        private static void NewFileUISlot_BetterSaves_SetFileToThis_Postfix()
        {
            IOController.RemoveConfigs();
            IOController.SetConfigCache();
            MenuController.SetLocked(true);
        }
    }
}
