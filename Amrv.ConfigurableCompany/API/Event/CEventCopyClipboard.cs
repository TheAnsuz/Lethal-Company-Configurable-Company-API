using Amrv.ConfigurableCompany.Utils.IO;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventCopyClipboard(ConfigBundle bundle) : CEvent
    {
        public readonly ConfigBundle Bundle = bundle;
    }
}
