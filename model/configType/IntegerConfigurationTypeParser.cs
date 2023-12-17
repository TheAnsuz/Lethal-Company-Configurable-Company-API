using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model.configType
{
    public sealed class IntegerConfigurationTypeParser : ConfigurationTypeParser
    {
        private const int DEFAULT = default;

        public override object Default() => DEFAULT;

        public override object Convert(object input)
        {
            return int.Parse(input.ToString());
        }

        public override bool IsValidType(object input)
        {
            return input is int;
        }

        public override bool TryConvert(object input, out object output)
        {
            if (int.TryParse(input.ToString(), out var result))
            {
                output = result;
                return true;
            }
            output = default;
            return false;
        }
    }
}
