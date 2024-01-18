using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    public class EnumConfigurationType : ListConfigurationType
    {
        public readonly Type EnumerationType;

        public EnumConfigurationType(Type enumeration) : base(OperateInput(enumeration))
        {
            EnumerationType = enumeration;
        }

        private static object[] OperateInput(Type enumeration)
        {
            if (enumeration == null || !enumeration.IsEnum)
                throw new ArgumentException("Can't create Enumertion configuration without an enumeration");

            var array = enumeration.GetEnumValues();
            object[] values = new object[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                values[i] = array.GetValue(i);
            }

            return values;
        }
    }
}
