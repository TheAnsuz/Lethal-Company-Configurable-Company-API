using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.types;

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
