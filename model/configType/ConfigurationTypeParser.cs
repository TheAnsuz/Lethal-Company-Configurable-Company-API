using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model.configType
{
    public abstract class ConfigurationTypeParser
    {
        public abstract object Default();

        public abstract bool TryConvert(object input, out object output);

        public abstract object Convert(object input);

        public abstract bool IsValidType(object input);
    }
}
