using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils.IO;
using Unity.Netcode;

namespace Amrv.ConfigurableCompany.Core.Net
{
    internal class NetReceiveRouter
    {
        public const string CONFIGS_SYNC = "configurable-company_client_configs-sync";

        public static void RegisterClientMessages()
        {
            ConfigurableCompanyPlugin.Debug($"NetReceiveRouter::RegisterClientMessages");
            NetSynchronizer.Messaging.Register(CONFIGS_SYNC, Client_ReceiveConfigs);
        }

        public static void RegisterServerMessages()
        {
            ConfigurableCompanyPlugin.Debug($"NetReceiveRouter::RegisterServerMessages");
        }

        internal static void Client_ReceiveConfigs(ulong senderClientId, FastBufferReader messagePayload)
        {
            ConfigurableCompanyPlugin.Debug($"NetReceiveRouter::Client_ReceiveConfigs {messagePayload.Length}");

            if (NetSynchronizer.IsServer)
                return;

            ConfigBundle bundle = new();

            messagePayload.ReadValueSafe(out string pack);
            bundle.Read(pack);

            if (!bundle.TryGetMetadata(nameof(NetController.VERSION), out string version) || !version.Equals(NetController.VERSION))
            {
                ConfigurableCompanyPlugin.Error($"Can't sync configurations, version mismatch {version} | {NetController.VERSION}");
                return;
            }

            foreach (var entries in bundle.Entries())
            {
                if (CConfig.Storage.TryGetValue(entries.Key, out var config) && config.Synchronized)
                {
                    ReadConfigBundle(config, entries.Value);
                }
            }
        }

        private static void ReadConfigBundle(CConfig config, ConfigBundle.ConfigEntry entry)
        {
            config.DeserializeValue(entry.Value, ChangeReason.PASTE_FROM_CLIPBOARD);

            if (entry.Metadata.TryGetValue("enabled", out string enabledString))
                config.Enabled = enabledString.ToLower().Equals("true");
        }
    }
}
