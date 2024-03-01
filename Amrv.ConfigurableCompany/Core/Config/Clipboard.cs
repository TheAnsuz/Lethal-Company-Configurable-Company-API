using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Utils.IO;
using System;
using System.IO;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Config
{
    internal static class Clipboard
    {
        public const string VERSION = "2c";

        public static void CopyToClipboard()
        {
            ConfigBundle bundle = new();
            bundle.AddMetadata(nameof(VERSION), VERSION);
            foreach (CConfig config in CConfig.Storage.Values)
            {
                WriteConfigBundle(config, bundle);
            }

            CEvents.IOSEvents.CopyToClipboard?.Invoke(new CEventCopyClipboard(bundle));

            bundle.Write(out string data);
            GUIUtility.systemCopyBuffer = data;
        }

        public static void PasteFromClipboard()
        {
            if (GUIUtility.systemCopyBuffer == null)
                return;

            ConfigBundle bundle = new();
            try
            {
                bundle.Read(GUIUtility.systemCopyBuffer);

                if (!bundle.TryGetMetadata(nameof(VERSION), out string version) || !version.Equals(VERSION))
                {
                    CEvents.IOSEvents.PasteFromClipboard?.Invoke(new CEventPasteClipboard(bundle, false, new IOException("Bundle was not of the correct version")));
                    return;
                }
            }
            catch (Exception e)
            {
                CEvents.IOSEvents.PasteFromClipboard?.Invoke(new CEventPasteClipboard(bundle, false, e));
                return;
            }

            foreach (CConfig config in CConfig.Storage.Values)
            {
                if (bundle.TryGetEntry(config.ID, out ConfigBundle.ConfigEntry entry))
                    ReadConfigBundle(config, entry);
                else
                    config.Reset(ChangeReason.PASTE_FROM_CLIPBOARD);
            }
        }

        private static void ReadConfigBundle(CConfig config, ConfigBundle.ConfigEntry entry)
        {
            config.DeserializeValue(entry.Value, ChangeReason.PASTE_FROM_CLIPBOARD);

            if (entry.Metadata.TryGetValue("enabled", out string enabledString))
                config.Enabled = enabledString.ToLower().Equals("true");
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
