using TMPro;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class IntegerDisplayType(long min, long max) : InputConfigDisplay
    {
        protected override TMP_InputField.ContentType ContentType => TMP_InputField.ContentType.IntegerNumber;

        protected readonly long Min = min;
        protected readonly long Max = max;

        protected override void OnEditEnd(string text)
        {
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
            UpdateModifiedState();
        }

        protected override void LoadFromConfig(in object config)
        {
            InputField.text = config.ToString();
        }

        protected override void SaveToConfig(out object value)
        {
            value = InputField.text;
        }

        protected override bool ValueEquals(in object original)
        {
            return InputField.text.Equals(original.ToString());
        }
    }
}
