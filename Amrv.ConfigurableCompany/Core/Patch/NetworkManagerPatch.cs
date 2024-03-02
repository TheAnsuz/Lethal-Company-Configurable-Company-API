using Amrv.ConfigurableCompany.Core.Net;
using Amrv.ConfigurableCompany.Plugin;
using HarmonyLib;
using Unity.Netcode;

namespace Amrv.ConfigurableCompany.Core.Patch
{
    [HarmonyPatch(typeof(NetworkManager))]
    internal class NetworkManagerPatch
    {
        [HarmonyPatch("Singleton", MethodType.Setter)]
        [HarmonyPostfix]
        private static void UpdateSingleton(NetworkManager value)
        {
            ConfigurableCompanyPlugin.Debug($"Detected NetworkManager singleton modification ({value == null}) {value}");

            if (value == null)
            {
                // The network manager should be destroyed
                // Should be null at this point, and anything attached to the variable would just flush with the GC. Right?
                NetSynchronizer.Destroy();
            }
            else
            {
                // The network manager should be created
                NetSynchronizer.Create(value);
            }
        }
    }
}
