using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace ConfigurableCompany.model
{
    public class ConfigurationSyncer
    {
        public const char ENTRY_SEPARATOR = '$';
        public const char KEY_VALUE_SEPARATOR = '%';

        private ConfigurationSyncer() { }

        public static string PackConfiguration()
        {
            StringBuilder stringBuilder = new();

            foreach (Configuration config in Configuration.Registered)
            {
                if (!config.Syncronized)
                    continue;

                ConfigurableCompanyPlugin.Debug($"Sending to sync: {config}");

                stringBuilder
                    .Append(config.ID)
                    .Append(KEY_VALUE_SEPARATOR)
                    .Append(config.GetRaw().ToString())
                    .Append(ENTRY_SEPARATOR);
            }

            return stringBuilder.ToString();
        }

        public static void ApplyPackedConfiguration(string info)
        {
            string[] entries = info.Split(ENTRY_SEPARATOR);
            string[] configdata;
            foreach (string entry in entries)
            {
                ConfigurableCompanyPlugin.Debug($"Reading from sync: {entry}");

                configdata = entry.Split(KEY_VALUE_SEPARATOR);
                if (Configuration.TryGet(configdata[0], out Configuration config))
                {
                    if (!config.Set(configdata[1], ChangeReason.SYNCRONIZATION))
                    {
                        ConfigurableCompanyPlugin.Error($"Tried to sync invalid value type {configdata[1]} for {config.ID}");
                    }
                }
                else
                {
                    ConfigurableCompanyPlugin.Error($"Tried to sync with unknown config ID: {configdata[0]}");
                }
            }
        }
    }
}
