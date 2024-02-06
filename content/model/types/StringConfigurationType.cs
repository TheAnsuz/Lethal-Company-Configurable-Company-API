using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class StringConfigurationType : ConfigurationType
    {
        private const string Default = "";

        public override object DefaultValue => Default;

        private readonly string _name;
        public override string Name => _name;

        public const int DefaultLength = 32;
        public const int ForcedMaxLength = 48;

        public readonly int MaxLength;

        public StringConfigurationType(int maxLength = DefaultLength)
        {
            if (maxLength <= 0)
                MaxLength = DefaultLength;
            else
            if (maxLength > ForcedMaxLength)
                MaxLength = ForcedMaxLength;
            else
                MaxLength = maxLength;

            _name = $"Text ({MaxLength} characters)";
        }

        public override bool IsValidValue(object value)
        {
            return value != null && value.ToString().Length <= MaxLength;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            int length = value.ToString().Length;
            result = length > MaxLength ? value.ToString().Substring(0, MaxLength) : value.ToString().Substring(0, length);
            return true;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new StringConfiguration(config, 32);
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code)
        {
            result = default;
            return false;
        }
    }
}
