using ConfigurableCompany.model.configType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model
{
    public sealed class ConfigurationType
    {
        public readonly string Name;
        private readonly ConfigurationTypeParser _validator;

        private ConfigurationType(string name, ConfigurationTypeParser validator)
        {
            Name = name;
            _validator = validator;
        }

        public bool IsValid(object input) => _validator.IsValidType(input);

        public object Convert(object input) => _validator.Convert(input);

        public object Default() => _validator.Default();

        public bool TryConvert(object input, out object output) => _validator.TryConvert(input, out output);

        public readonly static ConfigurationType String = new("String", new StringConfigurationTypeParser());
        public readonly static ConfigurationType Boolean = new("Boolean", new BooleanConfigurationTypeParser());
        public readonly static ConfigurationType Integer = new("Integer", new IntegerConfigurationTypeParser());
        public readonly static ConfigurationType Float = new("Float", new FloatConfigurationTypeParser());
        public readonly static ConfigurationType Percent = new("Percent", new PercentConfigurationTypeParser());
    }
}
