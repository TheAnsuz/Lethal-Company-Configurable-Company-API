using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using Amrv.ConfigurableCompany.Utils;
using System;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class BooleanType : CType
    {
        public override object Default => false;

        public override string TypeName => "Boolean";

        protected internal override ConfigDisplay CreateDisplay => new BooleanDisplayType();

        protected internal override bool Deserialize(in string data, out object item)
        {
            item = data?.ToLower().Equals("true") ?? false;
            return true;
        }

        public override bool IsValidValue(object value)
        {
            return value is bool;
        }

        protected internal override bool Serialize(in object item, out string data)
        {
            data = item.ToString().ToLower() == "true" ? "true" : "false";
            return true;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatProvider = null)
        {
            if (value != null)
            {
                if (value.ToString().ToLower().Equals("true"))
                {
                    result = true;
                    return true;
                }
                else if (value.ToString().ToLower().Equals("false"))
                {
                    result = false;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null)
        {
            if (!TryConvert(value, out object parsed, formatProvider) || parsed is not bool boolean)
            {
                boolean = false;
            }

            if (value != null && NumberUtils.IsNumber<T>())
            {
                result = (T)Convert.ChangeType(boolean ? 1 : 0, code);
                return true;
            }
            else if (type == typeof(bool))
            {
                result = (T)(object)boolean;
                return true;
            }
            else if (type == typeof(string))
            {
                result = (T)(object)boolean.ToString();
                return true;
            }
            result = default;
            return false;
        }

        public override object GetRandomValue(RNGProvider random, CConfig config, InfoProvider info)
        {
            bool val = random.Bool();
            Console.WriteLine($"Generated: {val} for {config.ID}");
            return val;
        }
    }
}
