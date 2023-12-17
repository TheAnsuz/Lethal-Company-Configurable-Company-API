using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model.configType
{
    public sealed class BooleanConfigurationTypeParser : ConfigurationTypeParser
    {
        private const bool DEFAULT = default;

        public override object Default() => DEFAULT;

        public override object Convert(object input)
        {
            return bool.Parse(input.ToString());
        }

        public override bool IsValidType(object input)
        {
            return input is bool;
        }

        public override bool TryConvert(object input, out object output)
        {
            output = false;
            if (bool.TryParse(input.ToString(), out var result))
            {
                output = result;
                return true;
            }
            return false;
        }
    }
}
