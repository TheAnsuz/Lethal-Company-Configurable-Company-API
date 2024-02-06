using Amrv.ConfigurableCompany.content.display;
using Amrv.ConfigurableCompany.content.display.configTypes;
using System;
using System.Globalization;

namespace Amrv.ConfigurableCompany.content.model.types
{
    /// <summary>
    /// Las configuraciones guardan el indice y esto guarda la lista
    /// </summary>
    public class ListConfigurationType : ConfigurationType
    {
        public override string Name => "Options";

        public override object DefaultValue => Options[0];

        public readonly object[] Options;
        public readonly Type OptionsType;

        public ListConfigurationType(params object[] options)
        {
            if (options == null || options.Length < 1)
                throw new ArgumentException("Can't create List Configuration Type without options");

            OptionsType = options[0].GetType();

            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].GetType() != OptionsType)
                    throw new ArgumentException($"All values in the list must be of the same type [Found {options[i].GetType()} but expected {OptionsType}]");
            }
            Options = options;
        }

        public virtual int IndexOf(object value)
        {
            for (int i = 0; i < Options.Length; i++)
            {
                if (Options[i].Equals(value)) return i;
            }
            return -1;
        }

        public override bool IsValidValue(object value)
        {
            return value is int index && index >= 0 && index < Options.Length;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null)
        {
            // Si el valor es nulo
            if (value == null)
            {
                result = null;
                return false;
            }

            if (value.GetType() == OptionsType)
            {
                // Si el valor es un objeto del array
                int index = IndexOf(value);
                if (index >= 0)
                {
                    result = index;
                    return true;
                }
            }

            // Si el valor es un indice
            if (int.TryParse(value.ToString(), NumberStyles.Integer, formatter ?? CultureInfo.InvariantCulture, out int val))
            {
                result = val >= Options.Length ? Options.Length : val <= 0 ? 0 : val;
                return true;
            }

            result = default;
            return false;
        }

        public string[] StringList() => Array.ConvertAll(Options, x => x.ToString());

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config)
        {
            return new OptionsConfiguration(config, StringList());
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code)
        {
            // Can get as VALUE(selected index) or INDEX(int, default)
            if (type == OptionsType)
            {
                if (int.TryParse(value.ToString(), out int index))
                {
                    result = (T)Options[index];
                    return true;
                }
                result = default;
                return false;
            }

            result = default;
            return false;
        }
    }
}
