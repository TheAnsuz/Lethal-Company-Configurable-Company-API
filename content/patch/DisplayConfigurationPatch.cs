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
        public static bool Patch_HostButtonClick { get; internal set; } = true;
        public static bool Patch_ChangeSelectedFile_Pre { get; internal set; } = true;
        public static bool Patch_ChangeSelectedFile_Post { get; internal set; } = true;
        public static bool Patch_ConfirmHostButton_Post { get; internal set; } = true;
        public static bool Patch_QuitButton_Prefix { get; internal set; } = false;

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ClickHostButton))]
        [HarmonyPostfix]
        private static void HostButtonClick(ref MenuManager __instance)
        {
            if (!Patch_HostButtonClick)
                return;

            IngameMenu.SafeInstantiate(__instance);
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPrefix]
        private static void ChangeSelectedFile_Pre(SaveFileUISlot __instance)
        {
            if (!Patch_ChangeSelectedFile_Pre)
                return;

            if (IngameMenu.ShouldIgnoreFile(/*__instance*/))
                return;

            IngameMenu.SaveCurrentConfig();
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void ChangeSelectedFile_Post(SaveFileUISlot __instance)
        {
            if (!Patch_ChangeSelectedFile_Post)
                return;

            if (IngameMenu.ShouldIgnoreFile(__instance))
            {
                IngameMenu.SetVisible(false);
                return;
            }

            IngameMenu.SetVisible(true);
            IngameMenu.LoadCurrentConfig();
        }

        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ConfirmHostButton))]
        [HarmonyPostfix]
        private static void ConfirmHostButton_Post()
        {
            if (!Patch_ConfirmHostButton_Post)
                return;

            if (IngameMenu.ShouldIgnoreFile())
            {
                IngameMenu.ResetCurrentConfig();
            }
            else
            {
                IngameMenu.SaveCurrentConfig();
                IngameMenu.SaveCategories();
            }

            IngameMenu.Delete();
        }

        //[HarmonyPatch(typeof(MenuManager), "ClickQuitButton")]
        //[HarmonyPrefix]
        private static void QuitButton_Prefix()
        {
            if (!Patch_QuitButton_Prefix)
                return;

            IngameMenu.SaveCurrentConfig();
            IngameMenu.SaveCategories();
        }
    }
}
