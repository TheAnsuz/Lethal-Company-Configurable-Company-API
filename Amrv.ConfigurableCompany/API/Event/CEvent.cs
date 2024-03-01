namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEvent
    {
        public static readonly CEvent Empty = new();

        protected internal CEvent() { }
    }

    public static class CEventTypeEmpty
    {
        internal static void Invoke(this CEventType<CEvent> instance)
        {
            instance.Invoke(CEvent.Empty);
        }
    }
}
