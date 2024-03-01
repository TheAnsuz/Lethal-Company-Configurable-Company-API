using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils.IO;
using Unity.Netcode;

namespace Amrv.ConfigurableCompany.Core.Net
{
    internal static class NetController
    {
        public const string VERSION = "2n";

        public const string CONFIGS_SYNC = NetReceiveRouter.CONFIGS_SYNC;

        public static void SendAllConfigs(ulong client)
        {
            ConfigBundle bundle = new();
            bundle.AddMetadata(nameof(VERSION), VERSION);

            foreach (var config in CConfig.Storage.Values)
            {
                if (!config.Synchronized)
                    continue;

                WriteConfigBundle(config, bundle);
            }

            bundle.Write(out string message);
            ConfigurableCompanyPlugin.Debug($"Send configuration bundle with size {FastBufferWriter.GetWriteSize(message, false)}");
            FastBufferWriter writer = new(FastBufferWriter.GetWriteSize(message, false), Unity.Collections.Allocator.Temp);
            writer.WriteValueSafe(message);
            NetSynchronizer.Messaging.Send(CONFIGS_SYNC, writer, NetworkDelivery.ReliableFragmentedSequenced, client);
        }

        public static void SendConfig(params CConfig[] configs)
        {
            ConfigBundle bundle = new();
            bundle.AddMetadata(nameof(VERSION), VERSION);

            foreach (var config in configs)
            {
                if (!config.Synchronized)
                    continue;

                WriteConfigBundle(config, bundle);
            }

            bundle.Write(out string message);
            ConfigurableCompanyPlugin.Debug($"Send configuration bundle with size {FastBufferWriter.GetWriteSize(message, false)}");
            FastBufferWriter writer = new(FastBufferWriter.GetWriteSize(message, false), Unity.Collections.Allocator.Temp);
            writer.WriteValueSafe(message);
            NetSynchronizer.Messaging.Send(CONFIGS_SYNC, writer);
        }

        private static void WriteConfigBundle(CConfig config, ConfigBundle bundle)
        {
            if (config.SerializeValue(out string serialized))
                bundle.AddEntry(config.ID, serialized, new()
                {
                    ["enabled"] = config.Enabled.ToString()
                });
        }
    }
}
