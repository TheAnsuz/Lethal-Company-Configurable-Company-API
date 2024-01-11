using Amrv.ConfigurableCompany.content.display.configTypes;
using Amrv.ConfigurableCompany.content.display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class SliderConfigurationType : FloatConfigurationType
    {
        public SliderConfigurationType(float min, float max) : base(min, max)
        {
            _name = $"Slider ({min}:{max})";
        }

        private readonly string _name;
        public override string Name => _name;

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new SliderConfiguration(config, MinValue, MaxValue);
        }
    }
}
