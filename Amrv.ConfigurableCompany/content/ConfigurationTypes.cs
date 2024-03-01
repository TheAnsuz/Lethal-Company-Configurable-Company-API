using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.types;
using System;

namespace Amrv.ConfigurableCompany
{
    [Obsolete("Use CTypes")]
    public partial class ConfigurationTypes
    {
        public static readonly ConfigurationType String = new StringConfigurationType();

        public static readonly ConfigurationType SmallString = new SmallStringConfigurationType();

        public static readonly ConfigurationType Boolean = new BooleanConfigurationType();

        public static readonly ConfigurationType Percent = new PercentConfigurationType();

        public static readonly ConfigurationType Float = new FloatConfigurationType();

        public static readonly ConfigurationType Integer = new IntegerConfigurationType();

        public static ConfigurationType RangeInteger(int min, int max) => new IntegerConfigurationType(min, max);

        public static ConfigurationType RangeFloat(float min, float max) => new FloatConfigurationType(min, max);

        public static ConfigurationType Slider(float min, float max) => new SliderConfigurationType(min, max);

        public static ConfigurationType StringOfLength(int maxLength) => new StringConfigurationType(maxLength);

        public static ConfigurationType Options(Type enumeration) => new EnumConfigurationType(enumeration);

        public static ConfigurationType Options(params string[] options) => new ListConfigurationType(options);

        private ConfigurationTypes() { }
    }
}
