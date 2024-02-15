using Amrv.ConfigurableCompany.content.model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class ConfigurationShare
    {
        public static void CopyToClipboard()
        {
            ConfigurationBundle bundle = new DefaultConfigurationBundle();

            foreach (Configuration config in Configuration.Configs)
                bundle.Add(config.ID, config.ValueToString());

            GUIUtility.systemCopyBuffer = bundle.Pack();
        }

        public static void PasteFromClipboard()
        {
            if (GUIUtility.systemCopyBuffer == null)
                return;

            ConfigurationBundle bundle = new DefaultConfigurationBundle();
            bundle.Unpack(GUIUtility.systemCopyBuffer);

            foreach (Configuration config in Configuration.Configs)
            {
                if (bundle.TryGet(config.ID, out string value))
                    config.TrySet(value, ChangeReason.READ_FROM_CLIPBOARD);
                else
                    config.Reset(ChangeReason.READ_FROM_CLIPBOARD);
            }
        }

        private ConfigurationShare() { }
    }
}
