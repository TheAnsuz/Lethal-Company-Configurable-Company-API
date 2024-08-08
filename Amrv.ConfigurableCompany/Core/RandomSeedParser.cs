using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Core
{
    public static class RandomSeedParser
    {
        private static readonly Random selector = new();

        public static readonly Dictionary<char, int> CHAR_MAPPING;
        public static readonly char[] CHAR_ARRAY = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static RandomSeedParser()
        {
            CHAR_MAPPING = new Dictionary<char, int>(CHAR_ARRAY.Length);
            for (int i = 0; i < CHAR_ARRAY.Length; i++)
                CHAR_MAPPING[CHAR_ARRAY[i]] = i;
        }

        public static bool IsValidString(string str)
        {
            if (str.Length > 10)
                return false;

            for (int i = 0; i < str.Length; i++)
                if (!IsValidChar(str[i]))
                    return false;
            return true;
        }

        public static bool IsValidChar(char ch) => (/*Digits*/ch > 47 && ch < 58) || (/*Letters*/ch > 64 && ch < 91);

        public static string GenerateRandom() => GenerateRandom(selector);
        public static string GenerateRandom(Random random)
        {
            char[] slots = new char[10];
            for (int i = 0; i < slots.Length; i++)
                slots[i] = CHAR_ARRAY[random.Next(CHAR_ARRAY.Length)];

            return new string(slots);
        }

        /// <summary>
        /// 10 character long and
        /// base 36
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long FromSeed(string str)
        {
            str = FormalizeString(str);

            long value = 0;
            for (int i = 0; i < str.Length; i++)
            {
                value *= 36;
                value += CHAR_MAPPING.GetValueOrDefault(str[i], 0);
            }

            return value;
        }

        public static string FormalizeString(string str)
        {
            str = str.Trim();

            StringBuilder builder = new(Math.Min(10, str.Length));

            for (int i = 0; i < builder.Capacity; i++)
            {
                if (CHAR_MAPPING.TryGetValue(str[i], out _))
                    builder.Append(char.ToUpper(str[i]));
            }

            return builder.ToString();
        }
    }
}
