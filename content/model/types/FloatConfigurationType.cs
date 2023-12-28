using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class FloatConfigurationType : ConfigurationType
    {
        private const int Default = default;

        public override object DefaultValue => Default;

        public override string Name => "Decimal";

        public override bool IsValidValue(object value)
        {
            return value is float;
        }

        public override bool TryConvert(object value, out object result)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            if (float.TryParse(value.ToString(), out float val))
            {
                result = val;
                return true;
            }

            result = default;
            return false;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new FloatConfiguration(config);
        }
    }
}
