﻿namespace Amrv.ConfigurableCompany.content.utils
{
    public class DataUtils
    {
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

            if (c == '_') return true;
            if (c == '-') return true;

            return char.IsLetterOrDigit(c);
        }
    }
}
