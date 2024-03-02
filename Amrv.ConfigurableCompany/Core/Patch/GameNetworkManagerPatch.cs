using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Core.Net;
using HarmonyLib;

namespace Amrv.ConfigurableCompany.Core.Patch
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatch
    {
        [HarmonyPatch(nameof(GameNetworkManager.SaveGame))]
        [HarmonyPostfix]
        private static void SaveGameValues_Postfix()
        {
            if (NetSynchronizer.IsServer)
            {
                IOController.SetConfigCache();
                IOController.SaveConfigs();
            }
        }
    }
}
