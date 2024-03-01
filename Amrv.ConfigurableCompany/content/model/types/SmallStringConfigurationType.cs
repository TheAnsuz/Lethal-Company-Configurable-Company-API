using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.content.display;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    [Obsolete("Use CTypes.String()")]
    public class SmallStringConfigurationType(int maxLength = 10) : ConfigurationType
    {
        private readonly CType _ctype = CTypes.String(maxLength);
        internal override CType CType => _ctype;

        public override object DefaultValue => CType.Default;

        public override string Name => CType.TypeName;

        public override bool IsValidValue(object value) => CType.IsValidValue(value);

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null) => CType.TryConvert(value, out result, formatter);

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code) => CType.TryGetAs(value, out result, type, code);

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config) => null;
    }
}
