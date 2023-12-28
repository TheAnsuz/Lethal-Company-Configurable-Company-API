using System;

namespace Amrv.ConfigurableCompany.content.model.events
{
    public class EventType<T> where T : EventArgs
    {
        public event EventHandler<T> Listeners;

        internal EventType() { }

        internal void Invoke(object sender, T args)
        {
            Listeners?.Invoke(sender, args);
        }

        internal void Invoke(T args)
        {
            Listeners?.Invoke(null, args);
        }

        public void AddListener(EventHandler<T> Listener)
        {
            Listeners += Listener;
        }

        public void RemoveListener(EventHandler<T> Listener)
        {
            Listeners -= Listener;
        }

        public static EventType<T> operator +(EventType<T> Event, EventHandler<T> Listener)
        {
            Event.Listeners += Listener;
            return Event;
        }

        public static EventType<T> operator -(EventType<T> Event, EventHandler<T> Listener)
        {
            Event.Listeners -= Listener;
            return Event;
        }
    }
}
