using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrv.ConfigurableCompany
{
    public partial class ConfigurationTypes
    {
        public readonly static ConfigurationType String = new StringConfigurationType();

        public readonly static ConfigurationType SmallString = new SmallStringConfigurationType();

        public readonly static ConfigurationType Boolean = new BooleanConfigurationType();

        public readonly static ConfigurationType Percent = new PercentConfigurationType();

        public readonly static ConfigurationType Float = new FloatConfigurationType();

        public readonly static ConfigurationType Integer = new IntegerConfigurationType();

        private ConfigurationTypes() { }
    }
}
