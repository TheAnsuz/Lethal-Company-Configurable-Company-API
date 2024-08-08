using Amrv.ConfigurableCompany.API.Data;
using System;

namespace Amrv.ConfigurableCompany.API
{
    public abstract class CRandomizer
    {
        public delegate object Randomize(RNGProvider rng, CConfig config, InfoProvider info);

        private static readonly CRandomizer _none = new CRandomizerNone();
        public static CRandomizer None() => _none;

        private static readonly CRandomizer _default = new CRandomizerDefault();
        public static CRandomizer Default() => _default;

        [Obsolete("Use Randomize delegate or extend CRandomizer")]
        public static CRandomizer Create(Func<RNGProvider, CConfig, object> oldProvider)
        {
            return new CRandomizerFunction(oldProvider);
        }
        public static CRandomizer Create(Randomize provider) => provider;
        public bool Active = true;

        public abstract object Generate(RNGProvider provider, CConfig config, InfoProvider info = null);

        public static implicit operator CRandomizer(Randomize func)
        {
            return new CRandomizerFunction(func);
        }

        private class CRandomizerNone : CRandomizer
        {
            public CRandomizerNone()
            {
                Active = false;
            }

            public override object Generate(RNGProvider provider, CConfig config, InfoProvider info) => config.Default;
        }

        private class CRandomizerDefault : CRandomizer
        {
            public override object Generate(RNGProvider provider, CConfig config, InfoProvider info)
            {
                return config.Type.GetRandomValue(provider, config, info);
            }
        }

        private class CRandomizerFunction : CRandomizer
        {
            private readonly Randomize _func;

            [Obsolete("Use Randomize delegate or extend CRandomizer")]
            public CRandomizerFunction(Func<RNGProvider, CConfig, object> oldFunction)
            {
                _func = delegate (RNGProvider provider, CConfig config, InfoProvider info) { return oldFunction?.Invoke(provider, config); };
                Active = oldFunction != null;
            }

            public CRandomizerFunction(Randomize function)
            {
                _func = function;
                Active = function != null;
            }

            public override object Generate(RNGProvider provider, CConfig config, InfoProvider info)
            {
                return _func?.Invoke(provider, config, info) ?? config.Default;
            }
        }
    }
}
