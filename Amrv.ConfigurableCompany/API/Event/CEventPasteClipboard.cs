using Amrv.ConfigurableCompany.Utils.IO;
using System;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventPasteClipboard(ConfigBundle bundle, bool success, Exception error) : CEvent
    {
        public readonly ConfigBundle Bundle = bundle;
        public readonly bool Success = success;
        public readonly Exception Error = error;
    }
}