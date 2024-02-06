using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;
using Amrv.ConfigurableCompany.content.utils;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class BooleanConfigurationType : ConfigurationType
    {
        private const bool Default = default;

        public override object DefaultValue => Default;

        public override string Name => "Boolean";

        public override bool IsValidValue(object value)
        {
            return value is bool;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            if (bool.TryParse(value.ToString(), out bool val))
            {
                result = val;
                return true;
            }

            result = default;
            return false;
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code)
        {
            if (DataUtils.IsNumeric<T>())
            {
                result = (T)Convert.ChangeType(value.Equals(true) ? 1 : 0, code);
                return true;
            }
            result = default;
            return false;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new BooleanConfiguration(config);
        }
    }
}
