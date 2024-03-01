using TMPro;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class DecimalDisplayType(double min, double max) : InputConfigDisplay
    {
        protected override TMP_InputField.ContentType ContentType => TMP_InputField.ContentType.DecimalNumber;

        protected readonly double Min = min;
        protected readonly double Max = max;

        protected override void OnEditEnd(string text)
        {
            base.OnEditEnd(text);
            if (double.TryParse(text, out double value))
            {
                if (value < Min || value > Max)
                {
                    InputField.text = (value < Min ? Min : value > Max ? Max : value).ToString();
                }
            }
            else
            {
                InputField.text = Config.Value.ToString();
            }
        }

        protected internal override void LoadFromConfig(in object config)
        {
            InputField.text = config.ToString();
        }

        protected internal override void SaveToConfig(out object value)
        {
            value = InputField.text;
        }
    }
}
