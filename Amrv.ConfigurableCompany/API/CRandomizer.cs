using System;

namespace Amrv.ConfigurableCompany.API
{
    public abstract class CRandomizer
    {
        private static readonly CRandomizer _none = new CRandomizerNone();
        public static CRandomizer None() => _none;

        private static readonly CRandomizer _default = new CRandomizerFunction((RNGProvider provider, CConfig config) => config.Type.GetRandomValue(provider, config));
        public static CRandomizer Default() => _default;

        public static CRandomizer Create(Func<RNGProvider, CConfig, object> provider) => provider;
        public bool Active = true;

        public abstract object Generate(RNGProvider provider, CConfig config);

        public static implicit operator CRandomizer(Func<RNGProvider, CConfig, object> func)
        {
            return new CRandomizerFunction(func);
        }

        private class CRandomizerNone : CRandomizer
        {
            public CRandomizerNone()
            {
                Active = false;
            }

            public override object Generate(RNGProvider provider, CConfig config) => config.Default;
        }

        private class CRandomizerFunction : CRandomizer
        {
            private readonly Func<RNGProvider, CConfig, object> _func;

            public CRandomizerFunction(Func<RNGProvider, CConfig, object> function)
            {
                _func = function;
                Active = function != null;
            }

            public override object Generate(RNGProvider provider, CConfig config)
            {
                return _func?.Invoke(provider, config) ?? config.Default;
            }
        }
    }
}
