using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class SliderConfigurationType : FloatConfigurationType
    {
        public SliderConfigurationType(float min, float max) : base(min, max)
        {
            _name = $"Slider ( {min} : {max} )";
        }

        private readonly string _name;
        public override string Name => _name;

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new SliderConfiguration(config, MinValue, MaxValue);
        }
    }
}
