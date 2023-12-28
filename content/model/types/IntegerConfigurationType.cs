using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class IntegerConfigurationType : ConfigurationType
    {
        private const int Default = default;

        public override object DefaultValue => Default;

        public override string Name => "Whole number";

        public override bool IsValidValue(object value)
        {
            return value is int;
        }

        public override bool TryConvert(object value, out object result)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            if (int.TryParse(value.ToString(), out int val))
            {
                result = val;
                return true;
            }

            result = default;
            return false;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new IntegerConfiguration(config);
        }
    }
}
