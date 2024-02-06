using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;
using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Globalization;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class IntegerConfigurationType : ConfigurationType
    {
        public override object DefaultValue => MinValue;

        private readonly string _name;
        public override string Name => _name;

        public readonly int MinValue;
        public readonly int MaxValue;

        public IntegerConfigurationType()
        {
            MinValue = int.MinValue;
            MaxValue = int.MaxValue;
            _name = "Whole number";
        }

        public IntegerConfigurationType(int min, int max)
        {
            MinValue = min;
            MaxValue = max;

            _name = $"Whole number ( {MinValue} : {MaxValue} )";
        }

        public override bool IsValidValue(object value)
        {
            return value is int i && i >= MinValue && i <= MaxValue;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null)
        {
            if (value == null)
            {
                result = MinValue;
                return false;
            }

            if (int.TryParse(value.ToString(), NumberStyles.Integer, formatter ?? CultureInfo.InvariantCulture, out int val))
            {
                result = val >= MaxValue ? MaxValue : val <= MinValue ? MinValue : val;
                return true;
            }

            result = default;
            return false;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new IntegerConfiguration(config, MinValue, MaxValue);
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code)
        {
            if (DataUtils.IsNumeric<T>())
            {
                result = (T)Convert.ChangeType(value, code);
                return true;
            }

            result = default;
            return false;
        }
    }
}
