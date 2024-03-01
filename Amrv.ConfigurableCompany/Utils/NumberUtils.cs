using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Amrv.ConfigurableCompany.Utils
{
    public static class NumberUtils
    {
        private static readonly HashSet<Type> DecimalTypes = [
            typeof(double),
            typeof(float),
            typeof(decimal),
        ];

        private static readonly HashSet<Type> WholeNumberTypes = [
            typeof(int),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(ushort),
            typeof(uint),
        ];

        private static readonly HashSet<Type> NumericTypes = [
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(ushort),
            typeof(uint),
            typeof(float)
        ];

        public static bool IsWhole(object any) => WholeNumberTypes.Contains(any.GetType());
        public static bool IsWhole(Type type) => WholeNumberTypes.Contains(type);
        public static bool IsWhole<T>() => WholeNumberTypes.Contains(typeof(T));

        public static bool IsDecimal(object any) => DecimalTypes.Contains(any.GetType());
        public static bool IsDecimal(Type type) => DecimalTypes.Contains(type);
        public static bool IsDecimal<T>() => DecimalTypes.Contains(typeof(T));

        public static bool IsNumber(object any)
        {
            return NumericTypes.Contains(any.GetType());
        }

        public static bool IsNumber(Type type) => NumericTypes.Contains(type);

        public static bool IsNumber<T>()
        {
            return NumericTypes.Contains(Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
        }

        private const double THOUSAND = 1000;
        private const double MILLION = 1000000;
        private const double BILLION = 1000000000;
        private const double TRILLION = 1000000000000;
        private const double QUADRILLION = 1000000000000000;
        private const double QUINTILLION = 1000000000000000000;
        private const double TOO_MUCH = 1000000000000000000000d;

        /// <summary>
        /// Converts the number into a formatted text that rounds the number with an unit, this can include K for kilos, M for millions and so on
        /// </summary>
        /// <param name="num">The number to convert</param>
        /// <returns>A string representation of the number with a sufix indicating how large is the number</returns>
        public static string NumberWithSuffix(double num)
        {
            if (num == double.MinValue || num == double.NegativeInfinity)
                return "-Infinite";
            else if (num == double.MaxValue || num == double.PositiveInfinity)
                return "Infinite";

            if (num == 0)
                return "0";

            string symbol = num < 0 ? "-" : "";
            num = Math.Abs(num);

            if (num > TOO_MUCH)
                return $"{symbol}Infinite";
            if (num > QUINTILLION)
                return $"{symbol}{num / QUINTILLION:#.00}E";
            else if (num > QUADRILLION)
                return $"{symbol}{num / QUADRILLION:#.00}Q";
            else if (num > TRILLION)
                return $"{symbol}{num / TRILLION:#.00}T";
            else if (num > BILLION)
                return $"{symbol}{num / BILLION:#.00}B";
            else if (num > MILLION)
                return $"{symbol}{num / MILLION:#.00}M";
            else if (num > THOUSAND)
                return $"{symbol}{num / THOUSAND:#.00}K";
            return $"{symbol}{num:#}";
        }

        public static string NumberWithSuffix(long num)
        {
            if (num == long.MinValue)
                return "-Infinite";
            else if (num == long.MaxValue)
                return "Infinite";

            if (num == 0)
                return "0";

            string symbol = num < 0 ? "-" : "";
            num = Math.Abs(num);

            if (num > TOO_MUCH)
                return $"{symbol}Infinite";
            if (num > QUINTILLION)
                return $"{symbol}{num / QUINTILLION:#}E";
            else if (num > QUADRILLION)
                return $"{symbol}{num / QUADRILLION:#}Q";
            else if (num > TRILLION)
                return $"{symbol}{num / TRILLION:#}T";
            else if (num > BILLION)
                return $"{symbol}{num / BILLION:#}B";
            else if (num > MILLION)
                return $"{symbol}{num / MILLION:#}M";
            else if (num > THOUSAND)
                return $"{symbol}{num / THOUSAND:#}K";
            return $"{symbol}{num:#}";
        }
    }
}
