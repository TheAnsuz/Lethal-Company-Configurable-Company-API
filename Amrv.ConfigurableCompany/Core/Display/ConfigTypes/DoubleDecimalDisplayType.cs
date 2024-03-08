using System;
using TMPro;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class DoubleDecimalDisplayType((double min, double max) range) : DoubleConfigDisplay
    {
        protected readonly (double min, double max) Range = range;

        protected double LeftCurrent = range.min;
        protected double RightCurrent = range.max;

        protected override TMP_InputField.ContentType LeftContentType => TMP_InputField.ContentType.IntegerNumber;

        protected override TMP_InputField.ContentType RightContentType => TMP_InputField.ContentType.IntegerNumber;

        protected override void OnLeftEditEnd(string text)
        {
            base.OnLeftEditEnd(text);
            if (double.TryParse(text, out double value))
            {
                LeftCurrent = value < Range.min ? Range.min : value > RightCurrent ? RightCurrent : value;
            }
            LeftInput.text = LeftCurrent.ToString();
        }

        protected override void OnRightEditEnd(string text)
        {
            base.OnLeftEditEnd(text);
            if (double.TryParse(text, out double value))
            {
                RightCurrent = value < LeftCurrent ? LeftCurrent : value > Range.max ? Range.max : value;
            }
            RightInput.text = RightCurrent.ToString();
        }

        protected internal override void LoadFromConfig(in object value)
        {
            if (value is (double, double))
            {
                (double, double) tuple = ((double, double))value;
                LeftCurrent = tuple.Item1;
                RightCurrent = tuple.Item2;
            }
            RightInput.text = RightCurrent.ToString();
            LeftInput.text = LeftCurrent.ToString();
        }

        protected internal override void SaveToConfig(out object value)
        {
            value = (LeftCurrent, RightCurrent);
        }
    }
}
