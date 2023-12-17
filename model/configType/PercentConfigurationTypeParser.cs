using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model.configType
{
    public sealed class PercentConfigurationTypeParser : ConfigurationTypeParser
    {
        private const float DEFAULT = default;

        public override object Default() => DEFAULT;

        public override object Convert(object input)
        {
            float val = float.Parse(input.ToString());
            return val > 100 ? 100 : val < 0 ? 0 : val;
        }

        public override bool IsValidType(object input)
        {
            return input is float;
        }

        public override bool TryConvert(object input, out object output)
        {
            if (float.TryParse(input.ToString(), out var result))
            {
                output = result > 100 ? 100 : result < 0 ? 0 : result;
                return true;
            }
            output = default;
            return false;
        }
    }
}
