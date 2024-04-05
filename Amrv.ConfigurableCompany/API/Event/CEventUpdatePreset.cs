using Amrv.ConfigurableCompany.Core.Config;
using System;
using System.Collections.Generic;
using System.Text;
using static Amrv.ConfigurableCompany.API.Event.CEventUpdatePreset;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventUpdatePreset(string file, PresetAction action) : CEvent
    {
        public string Folder => Presets.Folder;
        public enum PresetAction
        {
            CREATE,
            DELETE,
            UPDATE,
            STABLISH,
        }
    }
}
