using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class PercentConfigurationType : ConfigurationType
    {
        private const float MAX = 100f;
        private const float MIN = 0f;
        private const int Default = default;

        public override object DefaultValue => Default;

        public override string Name => "Percent";

        public override bool IsValidValue(object value)
        {
            if (value is float num)
                return num >= 0f && num <= 100f;
            return false;
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
                result = val >= MAX ? MAX : val <= MIN ? MIN : val;
                return true;
            }

            result = default;
            return false;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new SliderConfiguration(config);
        }
    }
}
