using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

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

        public override bool TryConvert(object value, out object result)
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

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new BooleanConfiguration(config);
        }
    }
}
