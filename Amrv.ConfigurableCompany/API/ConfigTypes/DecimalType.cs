using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Globalization;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class DecimalType : CType
    {
        protected readonly double Max;
        protected readonly double Min;

        protected readonly string _typename;
        public override string TypeName => _typename;

        public override object Default => default;

        protected internal override ConfigDisplay CreateDisplay => UseSlider ? new SliderDisplayType((float)Min, (float)Max) : new DecimalDisplayType(Min, Max);

        public bool UseSlider = false;

        public DecimalType(double min = double.MinValue, double max = double.MaxValue)
        {
            Min = min;
            Max = max;
            _typename = "Decimal number";

            if (Min != double.MinValue || Max != double.MaxValue)
            {
                _typename += $" | {(min != double.MinValue ? "Min: " + NumberUtils.NumberWithSuffix(min) + " " : "")}{(max != double.MaxValue ? "Max: " + NumberUtils.NumberWithSuffix(max) : "")}";
            }
        }

        public override bool IsValidValue(object value)
        {
            return value is double number && number >= Min && number <= Max;
        }


        public override bool TryConvert(object value, out object result, IFormatProvider formatProvider = null)
        {
            if (value != null && double.TryParse(value.ToString(), NumberStyles.Float, formatProvider, out double numberResult))
            {
                result = numberResult > Max ? Max : numberResult < Min ? Min : numberResult;
                return true;
            }

            result = default;
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
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                item = result;
                return true;
            }

            item = default;
            return false;
        }

        protected internal override bool Serialize(in object item, out string data)
        {
            if (item is double l)
            {
                data = l.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            else if (NumberUtils.IsNumber(item))
            {
                l = (double)Convert.ChangeType(item, TypeCode.Double, CultureInfo.InvariantCulture);
                data = l.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            data = default;
            return false;
        }
    }
}
