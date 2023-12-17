using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model.configType
{
    public sealed class StringConfigurationTypeParser : ConfigurationTypeParser
    {
        private const string DEFAULT = default;

        public override object Default() => DEFAULT;

        public override object Convert(object input)
        {
            return input.ToString();
        }

        public override bool IsValidType(object input)
        {
            return input is string;
        }

        public override bool TryConvert(object input, out object output)
        {
            if (bool.TryParse(input.ToString(), out var result))
            {
                output = result;
                return true;
            }
            output = default;
            return false;
        }
    }
}
