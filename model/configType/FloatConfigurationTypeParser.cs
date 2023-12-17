using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model.configType
{
    public sealed class FloatConfigurationTypeParser : ConfigurationTypeParser
    {
        private const float DEFAULT = default;

        public override object Default() => DEFAULT;

        public override object Convert(object input)
        {
            return float.Parse(input.ToString());
        }

        public override bool IsValidType(object input)
        {
            return input is float;
        }

        public override bool TryConvert(object input, out object output)
        {
            if (float.TryParse(input.ToString(), out var result))
            {
                output = result;
                return true;
            }
            output = default;
            return false;
        }
    }
}
