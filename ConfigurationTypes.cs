using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.types;

namespace Amrv.ConfigurableCompany
{
    public partial class ConfigurationTypes
    {
        public static readonly ConfigurationType String = new StringConfigurationType();

        public static readonly ConfigurationType SmallString = new SmallStringConfigurationType();

        public static readonly ConfigurationType Boolean = new BooleanConfigurationType();

        public static readonly ConfigurationType Percent = new PercentConfigurationType();

        public static readonly ConfigurationType Float = new FloatConfigurationType();

        public static readonly ConfigurationType Integer = new IntegerConfigurationType();

        private ConfigurationTypes() { }
    }
}
