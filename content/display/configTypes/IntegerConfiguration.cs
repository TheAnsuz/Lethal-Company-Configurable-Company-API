using Amrv.ConfigurableCompany.content.model;
using System.Globalization;
using TMPro;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public sealed class IntegerConfiguration : SmallInputConfiguration
    {
        public readonly int MinValue;
        public readonly int MaxValue;

        public IntegerConfiguration(Configuration Config, int min, int max) : base(Config, TMP_InputField.ContentType.IntegerNumber)
        {
            MinValue = min;
            MaxValue = max;
        }

        protected override bool ValidateText(string text)
        {
            return int.TryParse(text, out int value) && value >= MinValue && value <= MaxValue;
        }

        protected override void GetFromConfig(Configuration Config)
        {
            base.GetFromConfig(Config);
            InputArea_Input.text = Config.Value.ToString();
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(InputArea_Input.text, model.data.ChangeReason.USER_CHANGED, CultureInfo.CurrentCulture);
        }
    }
}
