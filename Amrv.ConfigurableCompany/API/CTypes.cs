using Amrv.ConfigurableCompany.API.ConfigTypes;
using System;

namespace Amrv.ConfigurableCompany.API
{
    public static class CTypes
    {
        internal static void Ping() { }

        static CTypes()
        {
            CType.SetMapping<bool>(_boolean);

            CType.SetMapping<double>(_decimalAny);
            CType.SetMapping<decimal>(_decimalAny);
            CType.SetMapping<float>(_decimalAny);

            CType.SetMapping<sbyte>(_integerAny);
            CType.SetMapping<byte>(_integerAny);
            CType.SetMapping<short>(_integerAny);
            CType.SetMapping<ushort>(_integerAny);
            CType.SetMapping<int>(_integerAny);
            CType.SetMapping<uint>(_integerAny);
            CType.SetMapping<long>(_integerAny);
            CType.SetMapping<ulong>(_integerAny);

            CType.SetMapping<string>(_string);

            CType.SetMapping<(sbyte, sbyte)>(_intRange);
            CType.SetMapping<(byte, byte)>(_intRange);
            CType.SetMapping<(short, short)>(_intRange);
            CType.SetMapping<(ushort, ushort)>(_intRange);
            CType.SetMapping<(int, int)>(_intRange);
            CType.SetMapping<(uint, uint)>(_intRange);
            CType.SetMapping<(long, long)>(_intRange);
            CType.SetMapping<(ulong, ulong)>(_intRange);

            CType.SetMapping<(double, double)>(_decimalRange);
            CType.SetMapping<(decimal, decimal)>(_decimalRange);
            CType.SetMapping<(float, float)>(_decimalRange);
        }

        private static readonly CType _decimalAny = new DecimalType();
        private static readonly CType _decimalPositive = new DecimalType(min: 0);
        private static readonly CType _integerAny = new IntegerType();
        private static readonly CType _integerPositive = new IntegerType(min: 0);
        private static readonly CType _boolean = new BooleanType();
        private static readonly CType _string = new StringType();
        private static readonly CType _decimalPercent = new DecimalType(0f, 100f) { UseSlider = true };
        private static readonly CType _intPercent = new IntegerType(0, 100) { UseSlider = true };
        private static readonly CType _intRange = new IntegerRangeType((long.MinValue, long.MaxValue));
        private static readonly CType _decimalRange = new DecimalRangeType((double.MinValue, double.MaxValue));

        public static CType DecimalNumberRange() => _decimalRange;
        public static CType DecimalNumberRange(double min, double max) => new DecimalRangeType((min, max));

        public static CType DecimalNumber() => _decimalAny;
        public static CType DecimalNumberPositive() => _decimalPositive;
        public static CType DecimalNumber(double min, double max) => new DecimalType(min, max);

        public static CType WholeNumberRange() => _intRange;
        public static CType WholeNumberRange(long min, long max) => new IntegerRangeType((min, max));

        public static CType WholeNumber() => _integerAny;
        public static CType WholeNumberPositive() => _integerPositive;
        public static CType WholeNumber(long min, long max) => new IntegerType(min, max);

        public static CType DecimalPercent() => _decimalPercent;
        public static CType DecimalSlider(float min, float max) => new DecimalType(min, max) { UseSlider = true };

        public static CType WholePercent() => _intPercent;
        public static CType WholeSlider(int min, int max) => new IntegerType(min, max) { UseSlider = true };

        public static CType Boolean() => _boolean;

        public static CType ArraySingleOption(Array array) => new ArraySingleType(array);
        public static CType ArraySingleOption<T>(params T[] array) => new ArraySingleType(array);
        public static CType EnumSinlgeOption<T>() where T : Enum => new ArraySingleType(typeof(T).GetEnumValues());

        public static CType String() => _string;
        public static CType String(int maxLength) => new StringType(maxLength);
    }
}
