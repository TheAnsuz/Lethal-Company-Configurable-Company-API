namespace Amrv.ConfigurableCompany.API.Event
{
    public delegate void CEventHandler<T>(T @event) where T : CEvent;

    public class CEventType<T> where T : CEvent
    {
        private event CEventHandler<T> Listeners;

        protected internal CEventType() { }

        protected internal void Invoke(T args)
        {
            Listeners?.Invoke(args);
        }

        public void AddListener(CEventHandler<T> Listener)
        {
            Listeners += Listener;
        }

        public void RemoveListener(CEventHandler<T> Listener)
        {
            Listeners -= Listener;
        }

        public static CEventType<T> operator +(CEventType<T> Event, CEventHandler<T> Listener)
        {
            Event.Listeners += Listener;
            return Event;
        }

        public static CEventType<T> operator -(CEventType<T> Event, CEventHandler<T> Listener)
        {
            Event.Listeners -= Listener;
            return Event;
        }
    }
}
