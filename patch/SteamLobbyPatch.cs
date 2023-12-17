using ConfigurableCompany.model;
using HarmonyLib;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.patch
{
    internal class SteamLobbyPatch
    {
        private SteamLobbyPatch() { }

        [HarmonyPatch(typeof(GameNetworkManager))]
        [HarmonyPatch("SteamMatchmaking_OnLobbyCreated")]
        [HarmonyPostfix]
        static void GameNetworkManager_SteamMatchmaking_OnLobbyCreated_Postfix(GameNetworkManager __instance, ref Result result, ref Lobby lobby)
        {
            if (result != Result.OK)
            {
                ConfigurableCompanyPlugin.Error("Can't find steam lobby to sync configuration");
                return;
            }

            lobby.SetData("amrv_config_sync", ConfigurationSyncer.PackConfiguration());
        }

        [HarmonyPatch(typeof(GameNetworkManager))]
        [HarmonyPatch(nameof(GameNetworkManager.JoinLobby))]
        [HarmonyPostfix]
        static void GameNetworkManager_JoinLobby_Postfix(GameNetworkManager __instance, ref Lobby lobby, ref SteamId id)
        {
            if (__instance.currentLobby.HasValue)
            {
                string data = lobby.GetData("amrv_config_sync");

                if (data == null || data.Length == 0)
                {
                    ConfigurableCompanyPlugin.Info("No configuration to sync");
                }
                else
                {
                    ConfigurationSyncer.ApplyPackedConfiguration(data);
                }
            }
            else
            {
                ConfigurableCompanyPlugin.Error("No lobby value to sync");
            }
        }
    }
}
