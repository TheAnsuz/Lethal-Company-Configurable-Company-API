using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.content.display;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    [Obsolete("Use CTypes.DecimalPercent()")]
    public class PercentConfigurationType : FloatConfigurationType
    {
        private readonly CType _ctype;
        internal override CType CType => _ctype;

        public PercentConfigurationType() : base(0, 100)
        {
            _ctype = CTypes.DecimalPercent();
        }

        public override string Name => "Percent";

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config) => null;
    }
}
