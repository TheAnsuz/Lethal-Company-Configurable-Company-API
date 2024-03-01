using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Globalization;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class IntegerType : CType
    {
        protected readonly long Max;
        protected readonly long Min;

        protected readonly long _default;
        public override object Default => _default;

        protected readonly string _typename;
        public override string TypeName => _typename;

        protected internal override ConfigDisplay CreateDisplay => UseSlider ? new SliderDisplayType((int)Min, (int)Max) : new IntegerDisplayType(Min, Max);

        public bool UseSlider = false;

        public IntegerType(long min = long.MinValue, long max = long.MaxValue, long defaultValue = default)
        {
            Min = min;
            Max = max;
            _default = defaultValue;
            _typename = "Whole number";
            if (Min != long.MinValue || Max != long.MaxValue)
            {
                _typename += $" ( {(min != long.MinValue ? "Min: " + NumberUtils.NumberWithSuffix(min) + " " : "")}{(max != long.MaxValue ? "Max: " + NumberUtils.NumberWithSuffix(max) : "")} )";
            }
        }

        public override bool IsValidValue(object value)
        {
            return value is long number && number >= Min && number <= Max;
        }


        public override bool TryConvert(object value, out object result, IFormatProvider formatProvider = null)
        {
            if (value != null && long.TryParse(value.ToString(), NumberStyles.Integer, formatProvider, out long numberResult))
            {
                result = numberResult > Max ? Max : numberResult < Min ? Min : numberResult;
                return true;
            }

            result = Default;
            return false;
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null)
        {
            if (typeof(string) == type || NumberUtils.IsNumber<T>())
            {
                result = (T)Convert.ChangeType(value, code, formatProvider);
                return true;
            }

            result = default;
            return false;
        }

        protected internal override bool Deserialize(in string data, out object item)
        {
            if (long.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
            {
                item = result;
                return true;
            }

            item = default;
            return false;
        }

        protected internal override bool Serialize(in object item, out string data)
        {
            if (item is long l)
            {
                data = l.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            else if (NumberUtils.IsNumber(item))
            {
                l = (long)Convert.ChangeType(item, TypeCode.Int64, CultureInfo.InvariantCulture);
                data = l.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            data = default;
            return false;
        }
    }
}
