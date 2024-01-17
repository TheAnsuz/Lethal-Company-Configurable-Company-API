using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class FloatConfigurationType : ConfigurationType
    {
        public override object DefaultValue => MinValue;

        private readonly string _name;
        public override string Name => _name;

        public readonly float MinValue;
        public readonly float MaxValue;

        public FloatConfigurationType()
        {
            MinValue = float.MinValue;
            MaxValue = float.MaxValue;
            _name = $"Decimal";
        }

        public FloatConfigurationType(float min, float max)
        {
            MinValue = min;
            MaxValue = max;

            _name = $"Decimal ( {MinValue} : {MaxValue} )";
        }

        public override bool IsValidValue(object value)
        {
            return value is float f && f >= MinValue && f <= MaxValue;
        }

        public override bool TryConvert(object value, out object result)
        {
            if (value == null)
            {
                result = MinValue;
                return false;
            }

            if (float.TryParse(value.ToString(), out float val))
            {
                result = val >= MaxValue ? MaxValue : val <= MinValue ? MinValue : val;
                return true;
            }

            result = default;
            return false;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new FloatConfiguration(config, MinValue, MaxValue);
        }
    }
}
