using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using System;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class StringType(int length = LargeInputConfigDisplay.MAX_CHARACTERS) : CType
    {
        public override object Default => "";

        public override string TypeName => "Text";

        protected internal override ConfigDisplay CreateDisplay => new StringDisplayType(length);

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
