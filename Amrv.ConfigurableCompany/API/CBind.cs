namespace Amrv.ConfigurableCompany.API
{
    public sealed class CBind<T>
    {
        public readonly CConfig Config;

        public bool Active => Config.Enabled;
        public T Default => Config.Type.TryGetAs(Default, out T val) ? val : default;

        internal CBind(CConfig config)
        {
            Config = config;
        }

        public T Value
        {
            get => Config.Get<T>();
            set => Config.TrySet(value);
        }

        public static implicit operator CBind<T>(CConfig config)
        {
            return config.Bind<T>();
        }

        public static implicit operator T(CBind<T> binder)
        {
            return binder.Value;
        }

        public static implicit operator CBind<T>(CConfigBuilder builder)
        {
            return builder.Build().Bind<T>();
        }
    }
}
