using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.content.display;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    [Obsolete("Use CTypes.DecimalSlider(min,max)")]
    public class SliderConfigurationType : FloatConfigurationType
    {
        private readonly CType _ctype;
        internal override CType CType => _ctype;

        public SliderConfigurationType(float min, float max) : base(min, max)
        {
            _name = $"Slider ( {min} : {max} )";
            _ctype = CTypes.DecimalSlider(min, max);
        }

        private readonly string _name;
        public override string Name => _name;

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config) => null;
    }
}
