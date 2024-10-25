using Amrv.ConfigurableCompany.Plugin;
using System;
using System.Collections;

namespace Amrv.ConfigurableCompany.API.Event
{
    public delegate void CEventHandler<T>(T @event) where T : CEvent;

    public class CEventType<T> where T : CEvent
    {
        private event CEventHandler<T> Listeners;

        protected internal CEventType() { }

        protected internal void InvokeFull(T args)
        {
            IEnumerator enumerator = Invoke(args);
            while (enumerator.MoveNext())
            {
            }
        }

        protected internal IEnumerator Invoke(T args)
        {
            if (Listeners == null)
                yield break;

            foreach (var listener in Listeners.GetInvocationList())
            {
                try
                {
                    listener.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    ConfigurableCompanyPlugin.Error(e);
                }
                yield return null;
            }

            yield break;
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
