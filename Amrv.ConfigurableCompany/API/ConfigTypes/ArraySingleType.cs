using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using System.Globalization;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class ArraySingleType : CType
    {
        public override object Default => Values[0];

        public override string TypeName => "Single Option";

        public readonly object[] Values;

        protected internal override ConfigDisplay CreateDisplay => new EnumDisplayType(Values);

        public ArraySingleType(params object[] arrayValues)
        {
            Values = arrayValues;
        }

        public ArraySingleType(IEnumerable enumValues)
        {
            Values = [.. enumValues];
        }

        public override bool IsValidValue(object value)
        {
            if (value is int vInt)
                return TryGetByIndex(vInt, out var _);

            return Contains(value) != -1;
        }

        public bool TryGetByIndex(int index, out object value)
        {
            if (index < 0 || index >= Values.Length)
            {
                value = null;
                return false;
            }

            value = Values[index];
            return true;
        }

        public int Contains(object value)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i].Equals(value)) return i;
            }
            return -1;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatProvider = null)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            if (value is int vInt && TryGetByIndex(vInt, out var obj))
            {
                result = obj;
                return true;
            }

            int index = Contains(value);
            if (index != -1)
            {
                result = index;
                return true;
            }

            result = default;
            return false;
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            // Si se pide el objeto
            if (value is int vInt && TryGetByIndex(vInt, out var obj))
            {
                result = (T)obj;
                return true;
            }

            // Si se pide el indice
            int index = Contains(value);
            if (index != -1)
            {
                result = (T)Convert.ChangeType(index, code, formatProvider);
                return true;
            }

            result = default;
            return false;
        }

        protected internal override bool Deserialize(in string data, out object item)
        {
            if (int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                item = result;
                return true;
            }

            item = default;
            return false;
        }

        protected internal override bool Serialize(in object item, out string data)
        {
            if (item is int l || TryGetAs(item, out l))
            {
                data = l.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            else if (NumberUtils.IsNumber(item))
            {
                l = (int)Convert.ChangeType(item, TypeCode.Int32, CultureInfo.InvariantCulture);
                data = l.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            data = default;
            return false;
        }
    }
}
