using Amrv.ConfigurableCompany.Plugin;
using System;

namespace Amrv.ConfigurableCompany.Core
{
    public abstract class InstanceBuilder<T> : IDisposable
    {
        private T instance;

        public virtual bool TryBuild(out T item)
        {
            try
            {
                item = Build();
                return true;
            }
            catch (Exception)
            {
                item = default;
                return false;
            }
        }

        public virtual T GetOrCreate()
        {
            if (TryGetExisting(out T item))
                return item;

            return Build();
        }

        public T Build()
        {
            try
            {
                return instance ??= BuildInstance();
            }
            catch (Exception e)
            {
                ConfigurableCompanyPlugin.Error($"Error creating entry{Environment.NewLine}{e}");
                return default;
            }
        }

        protected abstract T BuildInstance();

        protected abstract bool TryGetExisting(out T item);

        public virtual void Dispose()
        {
            Build();
        }

        public static implicit operator T(InstanceBuilder<T> builder)
        {
            return builder.Build();
        }
    }
}
