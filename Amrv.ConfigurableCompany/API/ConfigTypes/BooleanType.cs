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
            if (value != null && bool.TryParse(value.ToString(), out bool parsed))
            {
                result = parsed;
                return true;
            }

            result = default;
            return false;
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null)
        {
            if (value != null && NumberUtils.IsNumber<T>())
            {
                result = (T)Convert.ChangeType(value.Equals(true) ? 1 : 0, code);
                return true;
            }
            result = default;
            return false;
        }
    }
}
