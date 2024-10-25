using System.Collections;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEvent
    {
        public static readonly CEvent Empty = new();

        protected internal CEvent() { }
    }

    public static class CEventTypeEmpty
    {
        public static void InvokeFull(this CEventType<CEvent> instance)
        {
            instance.InvokeFull(CEvent.Empty);
        }

        internal static IEnumerator Invoke(this CEventType<CEvent> instance)
        {
            return instance.Invoke(CEvent.Empty);
        }
    }
}
