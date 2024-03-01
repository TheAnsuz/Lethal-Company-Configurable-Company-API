﻿using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.ConfigTypes;
using Amrv.ConfigurableCompany.content.display;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    [Obsolete("Use CTypes.EnumSinlgeOption()")]
    public class EnumConfigurationType : ListConfigurationType
    {
        private CType _ctype;
        internal override CType CType => _ctype;

        public EnumConfigurationType(Type enumeration)
        {
            _ctype = CTypes.ArraySingleOption(enumeration.GetEnumValues());
        }

        public override object DefaultValue => CType.Default;

        public override string Name => CType.TypeName;

        public override bool IsValidValue(object value) => CType.IsValidValue(value);

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null) => CType.TryConvert(value, out result, formatter);

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code) => CType.TryGetAs(value, out result, type, code);

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config) => null;
    }
}
