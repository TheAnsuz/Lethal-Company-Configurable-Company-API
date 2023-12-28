using Amrv.ConfigurableCompany.content.model;
using HarmonyLib;
using Steamworks;
using Steamworks.Data;

namespace Amrv.ConfigurableCompany.content.patch
{
    internal class SteamLobbyPatch
    {
        private const string DATA_KEY = "amrv_configurable_company_sync";

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

            lobby.SetData(DATA_KEY, ConfigurationSync.CreatePack());
        }

        [HarmonyPatch(typeof(GameNetworkManager))]
        [HarmonyPatch(nameof(GameNetworkManager.JoinLobby))]
        [HarmonyPostfix]
        static void GameNetworkManager_JoinLobby_Postfix(GameNetworkManager __instance, ref Lobby lobby, ref SteamId id)
        {
            if (__instance.currentLobby.HasValue)
            {
                string data = lobby.GetData(DATA_KEY);

                if (data == null || data.Length == 0)
                {
                    ConfigurableCompanyPlugin.Info("No configuration to sync");
                }
                else
                {
                    ConfigurationSync.ReadPack(data);
                }
            }
            else
            {
                ConfigurableCompanyPlugin.Error("No lobby value to sync");
            }
        }
    }
}
