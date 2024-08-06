using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using System;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class StringType(int length = LargeInputConfigDisplay.MAX_CHARACTERS) : CType
    {
        private static readonly char[] RANDOM_CHARS = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];

        public override object Default => "";

        public override string TypeName => "Text";

        protected internal override ConfigDisplay CreateDisplay => new StringDisplayType(length);

        public override object GetRandomValue(RNGProvider random, CConfig config, InfoProvider info)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = RANDOM_CHARS[random.Int(RANDOM_CHARS.Length)];
            }

            return new string(chars);
        }

        public override bool IsValidValue(object value)
        {
            return value is string str && str.Length < length;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatProvider = null)
        {
            if (value is string str)
            {
                if (str.Length < length)
                    result = value;
                else
                    result = str[..length];
                return true;
            }
            else
            {
                result = value.ToString()[..length];
                return true;
            }
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null)
        {
            if (!TryConvert(value, out object parsed, formatProvider) || parsed is not string str)
                str = value.ToString();

            if (type == typeof(string))
            {
                result = (T)(object)str;
                return true;
            }

            result = default;
            return false;
        }

        protected internal override bool Deserialize(in string data, out object item)
        {
            item = data;
            return true;
        }

        protected internal override bool Serialize(in object item, out string data)
        {
            data = item.ToString();
            return true;
        }
    }
}
