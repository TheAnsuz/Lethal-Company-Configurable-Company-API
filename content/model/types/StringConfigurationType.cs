using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class StringConfigurationType : ConfigurationType
    {
        private const string Default = "";

        public override object DefaultValue => Default;

        public override string Name => "Text";

        public override bool IsValidValue(object value)
        {
            return true;
        }

        public override bool TryConvert(object value, out object result)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            result = value.ToString();
            return true;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new StringConfiguration(config);
        }
    }
}
