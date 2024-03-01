using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.ConfigTypes;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Amrv.ConfigurableCompany.API.ConfigTypes
{
    public class IntegerRangeType : CType
    {
        private static readonly Type TYPE_CONVERTABLE = typeof(IConvertible);
        private static readonly Type TYPE_ITUPLE = typeof(ITuple);
        private static readonly Type TYPE_TUPLE_T_T = typeof(Tuple<,>);

        public readonly long Min;
        public readonly long Max;

        private readonly (long, long) _default;
        public override object Default => _default;

        private readonly string _typename;
        public override string TypeName => _typename;

        protected internal override ConfigDisplay CreateDisplay => new DoubleIntegerDisplayType((Min, Max));

        public IntegerRangeType(long min, long max) : this((min, max)) { }

        public IntegerRangeType((long min, long max) range)
        {
            Min = range.min;
            Max = range.max;
            _default = range;
            _typename = $"Whole number range | {NumberUtils.NumberWithSuffix(Min)} to {NumberUtils.NumberWithSuffix(Max)}";
        }

        public override bool IsValidValue(object value)
        {
            if (value is (long, long))
            {
                (long, long) tuple = ((long, long))value;
                return tuple.Item1 >= Min && tuple.Item2 <= Max && tuple.Item1 <= tuple.Item2;
            }
            return false;
        }

        private (long, long) Clamp((long, long) tuple)
        {
            tuple.Item2 = tuple.Item2 > Max ? Max : tuple.Item2;
            tuple.Item1 = tuple.Item1 < Min ? Min : tuple.Item1 > tuple.Item2 ? tuple.Item2 : tuple.Item1;
            return tuple;
        }

        public override bool TryConvert(object value, out object result, IFormatProvider formatProvider = null)
        {
            if (value is (long, long))
            {
                result = value;
                return true;
            }
            else if (value is ITuple tuple && tuple.Length >= 2)
            {
                if (NumberUtils.IsNumber(tuple[0]) && NumberUtils.IsNumber(tuple[1]))
                {
                    result = Clamp(((long)Convert.ChangeType(tuple[0], TypeCode.Int64), (long)Convert.ChangeType(tuple[1], TypeCode.Int64)));
                    return true;
                }
                else if (long.TryParse(tuple[0].ToString(), out long min) && long.TryParse(tuple[1].ToString(), out long max))
                {
                    result = Clamp((min, max));
                    return true;
                }
            }
            else if (value is IList array && array.Count >= 2)
            {
                if (NumberUtils.IsNumber(array[0]) && NumberUtils.IsNumber(array[1]))
                {
                    result = (Convert.ChangeType(array[0], TypeCode.Int64), Convert.ChangeType(array[1], TypeCode.Int64));
                    return true;
                }
                else if (long.TryParse(array[0].ToString(), out long min) && long.TryParse(array[1].ToString(), out long max))
                {
                    result = (min, max);
                    return true;
                }
            }

            result = default;
            return false;
        }

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null)
        {
            if (!TryConvert(value, out object safe))
            {
                result = default;
                return false;
            }

            (long min, long max) = ((long, long))safe;

            if (TYPE_CONVERTABLE.IsAssignableFrom(type))
            {
                result = (T)Convert.ChangeType(max - min, code, formatProvider);
                return true;
            }
            else if (TYPE_ITUPLE.IsAssignableFrom(type))
            {
                if (type.GenericTypeArguments.Length != 2)
                {
                    result = default;
                    return false;
                }

                dynamic a = null;
                dynamic b = null;

                if (TYPE_CONVERTABLE.IsAssignableFrom(type.GenericTypeArguments[0]))
                    a = Convert.ChangeType(min, type.GenericTypeArguments[0].UnderlyingSystemType);

                if (TYPE_CONVERTABLE.IsAssignableFrom(type.GenericTypeArguments[1]))
                    b = Convert.ChangeType(min, type.GenericTypeArguments[1].UnderlyingSystemType);

                //if (a != null && b != null)
                if (TYPE_TUPLE_T_T.IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    result = Tuple.Create(a, b);
                    return true;
                }
                else
                {
                    result = ValueTuple.Create(a, b);
                    return true;
                }
            }
            else if (type.IsSubclassOf(typeof(Array)) && TYPE_CONVERTABLE.IsAssignableFrom(type.GetElementType()))
            {
                dynamic array = Array.CreateInstance(type.GetElementType(), 2);
                array.SetValue(Convert.ChangeType(min, type.GetElementType()), 0);
                array.SetValue(Convert.ChangeType(max, type.GetElementType()), 1);
                result = array;
                return true;
            }

            result = default;
            return false;

        }

        protected internal override bool Deserialize(in string data, out object item)
        {
            string[] split = data.Split(":");
            if (split.Length == 2 && long.TryParse(split[0], out long min) && long.TryParse(split[1], out long max))
            {
                item = Clamp((min, max));
                return true;
            }
            item = default;
            return false;
        }

        protected internal override bool Serialize(in object item, out string data)
        {
            if (IsValidValue(item))
            {
                (long min, long max) = ((long, long))item;
                data = min.ToString(CultureInfo.InvariantCulture) + ":" + max.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            data = default;
            return false;
        }
    }
}
