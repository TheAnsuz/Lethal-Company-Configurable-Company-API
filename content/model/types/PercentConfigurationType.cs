using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class PercentConfigurationType : FloatConfigurationType
    {
        public PercentConfigurationType() : base(0, 100)
        {
        }

        public override string Name => "Percent";

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new SliderConfiguration(config, MinValue, MaxValue);
        }
    }
}
