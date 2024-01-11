using Amrv.ConfigurableCompany.content.model;
using TMPro;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public sealed class FloatConfiguration : SmallInputConfiguration
    {
        public readonly float MinValue;
        public readonly float MaxValue;

        public FloatConfiguration(Configuration Config, float min, float max) : base(Config, TMP_InputField.ContentType.DecimalNumber)
        {
            MinValue = min;
            MaxValue = max;
        }

        protected override bool ValidateText(string text)
        {
            return float.TryParse(text, out float value) && value >= MinValue && value <= MaxValue;
        }

        protected override void GetFromConfig(Configuration Config)
        {
            base.GetFromConfig(Config);
            InputArea_Input.text = Config.Value.ToString();
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(InputArea_Input.text, model.data.ChangeReason.USER_CHANGED);
        }
    }
}
