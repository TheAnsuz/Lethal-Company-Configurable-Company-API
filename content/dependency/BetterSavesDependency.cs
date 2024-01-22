﻿using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.patch;
using BepInEx;
using HarmonyLib;

namespace Amrv.ConfigurableCompany.content.dependency
{
    internal sealed class BetterSavesDependency : AbstractDependency
    {
        internal BetterSavesDependency() { }

        public const string PLUGIN_GUID = "LCBetterSaves";

        public const BepInDependency.DependencyFlags DEPENDENCY_TYPE = BepInDependency.DependencyFlags.SoftDependency;

        public override string PluginGUID => PLUGIN_GUID;

        public override void Instantiate(Harmony harmony)
        {
            DisplayConfigurationPatch.Patch_ChangeSelectedFile_Post = false;
            harmony.PatchAll(typeof(BetterSavesDependency));
        }

        [HarmonyPatch(typeof(SaveFileUISlot_BetterSaves))]
        [HarmonyPatch(nameof(SaveFileUISlot_BetterSaves.SetFileToThis))]
        [HarmonyPostfix]
        static void SaveFileUISlot_BetterSaves_SetFileToThis_Postfix(SaveFileUISlot_BetterSaves __instance)
        {
            if (IngameMenu.ShouldIgnoreFile(__instance.fileNum))
            {
                IngameMenu.SetVisible(false);
                return;
            }

            IngameMenu.SetVisible(true);
            IngameMenu.LoadCurrentConfig();
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void ChangeSelectedFile_Post(SaveFileUISlot __instance)
        {
            IngameMenu.SetVisible(!IngameMenu.ShouldIgnoreFile(__instance));
        }
    }
}