using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class SmallStringConfigurationType : ConfigurationType
    {
        private const string Default = "";

        public override object DefaultValue => Default;

        public override string Name => "Small text";

        public override bool IsValidValue(object value)
        {
            return value != null && value.ToString().Length <= 10;
        }

        public override bool TryConvert(object value, out object result)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            int length = value.ToString().Length;
            result = length > 10 ? value.ToString().Substring(0, 10) : value.ToString().Substring(0, length);
            return true;
        }

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new SmallStringConfiguration(config);
        }
    }
}
