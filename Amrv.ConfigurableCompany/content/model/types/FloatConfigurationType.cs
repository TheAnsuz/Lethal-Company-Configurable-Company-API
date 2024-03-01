using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.content.display;
using System;

namespace Amrv.ConfigurableCompany.content.model.types
{
    [Obsolete("Use CTypes.DecimalNumber(min, max)")]
    public class FloatConfigurationType : ConfigurationType
    {
        private readonly CType _ctype;
        internal override CType CType => _ctype;

        public override object DefaultValue => CType.Default;

        public override string Name => CType.TypeName;

        public FloatConfigurationType(float min = float.MinValue, float max = float.MaxValue)
        {
            _ctype = CTypes.DecimalNumber(min, max);
        }

        public override bool IsValidValue(object value) => CType.IsValidValue(value);

        public override bool TryConvert(object value, out object result, IFormatProvider formatter = null) => CType.TryConvert(value, out result, formatter);

        public override bool TryGetAs<T>(object value, out T result, Type type, TypeCode code) => CType.TryGetAs(value, out result, type, code);

        protected override ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config) => null;
    }
}
