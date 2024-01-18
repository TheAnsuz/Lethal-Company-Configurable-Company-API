using System.Collections.Generic;
using System;

namespace Amrv.ConfigurableCompany.content.utils
{
    public class DataUtils
    {
        private static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        private DataUtils() { }

        public static bool IsValidID(string id)
        {
            if (id == null)
                return false;

            for (int i = 0; i < id.Length; i++)
            {
                if (!IsValidIDChar(id[i]))
                    return false;
            }

            return true;
        }

        public static bool IsValidIDChar(char c)
        {
            if (c == ' ') return false;

            if (char.IsUpper(c)) return false;

            return c == '_' ? true : c == '-' ? true : char.IsLetterOrDigit(c);
        }

        public static bool IsNumeric<T>()
        {
            return NumericTypes.Contains(Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
        }
    }
}
