using Amrv.ConfigurableCompany.Core.Config;
using static Amrv.ConfigurableCompany.API.Event.CEventUpdatePreset;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventUpdatePreset(string file, PresetAction action) : CEvent
    {
        public string Folder => Presets.Folder;
        public readonly string File = file;
        public readonly PresetAction Action = action;
        public enum PresetAction
        {
            CREATE,
            DELETE,
            UPDATE,
            STABLISH,
        }
    }
}
