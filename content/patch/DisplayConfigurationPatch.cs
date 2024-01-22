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
        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ClickHostButton))]
        [HarmonyPostfix]
        private static void HostButtonClick(ref MenuManager __instance)
        {
            IngameMenu.SafeInstantiate(__instance);
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPrefix]
        private static void ChangeSelectedFile_Pre(SaveFileUISlot __instance)
        {
            if (IngameMenu.ShouldIgnoreFile(/*__instance*/))
                return;

            IngameMenu.SaveCurrentConfig();
        }

        [HarmonyPatch(typeof(SaveFileUISlot), nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void ChangeSelectedFile_Post(SaveFileUISlot __instance)
        {
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
            IngameMenu.SaveCurrentConfig();
            IngameMenu.SaveCategories();
        }
    }
}
