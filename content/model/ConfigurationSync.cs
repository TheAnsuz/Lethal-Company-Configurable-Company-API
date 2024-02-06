using Amrv.ConfigurableCompany.content.model.data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class ConfigurationSync
    {
        private ConfigurationSync() { }

        public static string CreatePack()
        {
            ConfigurationBundle bundle = new DefaultConfigurationBundle();

            foreach (Configuration config in Configuration.Configs)
            {
                if (config.Synchronized)
                    bundle.Add(config.ID, config.ValueToString());
            }

            try
            {
                return bundle.Pack();
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to pack sync bundle", ex);
            }
        }

        public static void ReadPack(string pack)
        {
            ConfigurationBundle bundle = new DefaultConfigurationBundle();

            try
            {
                bundle.Unpack(pack);
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to unpack sync bundle", ex);
            }

            foreach (KeyValuePair<string, string> syncConfig in bundle)
            {
                if (Configuration.TryGet(syncConfig.Key, out Configuration value))
                {
                    bool result = value.TrySet(syncConfig.Value, ChangeReason.SYNCHRONIZED);
#if DEBUG
                    Console.WriteLine($"Syncing bundle key {syncConfig.Key}:{syncConfig.Value} => {result}");
#endif
                }
                else
                {
                    ConfigurableCompanyPlugin.Error($"Can't synchronize key {syncConfig.Key}");
                }
            }
        }
    }
}
