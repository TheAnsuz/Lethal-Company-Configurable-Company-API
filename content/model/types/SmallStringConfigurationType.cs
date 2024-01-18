using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class SmallStringConfigurationType : ConfigurationType
    {
        private const string Default = "";

        public override object DefaultValue => Default;

        private readonly string _name;
        public override string Name => _name;

        public const int DefaultLength = 10;
        public const int ForcedMaxLength = 10;

        public readonly int MaxLength;

        public SmallStringConfigurationType(int maxLength = DefaultLength)
        {
            if (maxLength <= 0)
                MaxLength = DefaultLength;
            else
            if (maxLength > ForcedMaxLength)
                MaxLength = ForcedMaxLength;
            else
                MaxLength = maxLength;

            _name = $"Short text ({MaxLength} characters)";
        }

        public override bool IsValidValue(object value)
        {
            return value != null && value.ToString().Length <= MaxLength;
        }

        public override bool TryConvert(object value, out object result)
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
            return new SmallStringConfiguration(config);
        }
    }
}
