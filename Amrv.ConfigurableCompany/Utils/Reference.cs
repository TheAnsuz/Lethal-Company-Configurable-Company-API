using System;

namespace Amrv.ConfigurableCompany.Utils
{
    public class Reference<T> : IDisposable where T : class
    {
        public Reference()
        {
            IsAlive = false;
        }

        public Reference(T item)
        {
            Item = item;
        }

        private readonly object _lock = new();
        public virtual bool IsAlive { get; private set; } = true;
        private T _value;

        public virtual T Item
        {
            get
            {
                if (IsAlive)
                {
                    return _value;
                }
                else
                {
                    throw new NullReferenceException($"Tried to get reference item after breaking it's bond");
                }
            }

            set
            {
                lock (_lock)
                {
                    if (IsAlive && value == null)
                        Break();
                    else
                        Bind(value);
                }
            }
        }

        [Obsolete("Reference content may leak")]
        public static implicit operator T(Reference<T> reference) => reference.Item;

        public virtual void Break()
        {
            lock (_lock)
            {
                IsAlive = false;
                _value = null;
            }
        }

        public virtual void Bind(T item)
        {
            if (item == null)
            {
                Break();
                return;
            }
            lock (_lock)
            {
                IsAlive = true;
                _value = item;
            }
        }

        public virtual void Dispose()
        {
            Break();
        }

        ~Reference()
        {
            _value = null;
        }
    }
}
