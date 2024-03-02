using System;
using TMPro;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class DoubleIntegerDisplayType((long min, long max) range) : DoubleConfigDisplay
    {
        protected readonly (long min, long max) Range = range;

        protected long LeftCurrent = range.min;
        protected long RightCurrent = range.max;

        protected override TMP_InputField.ContentType LeftContentType => TMP_InputField.ContentType.IntegerNumber;

        protected override TMP_InputField.ContentType RightContentType => TMP_InputField.ContentType.IntegerNumber;

        protected override void OnLeftEditEnd(string text)
        {
            base.OnLeftEditEnd(text);
            if (long.TryParse(text, out long value))
            {
                LeftCurrent = value < Range.min ? Range.min : value > RightCurrent ? RightCurrent : value;
            }
            LeftInput.text = LeftCurrent.ToString();
        }

        protected override void OnRightEditEnd(string text)
        {
            base.OnLeftEditEnd(text);
            if (long.TryParse(text, out long value))
            {
                RightCurrent = value < LeftCurrent ? LeftCurrent : value > Range.max ? Range.max : value;
            }
            RightInput.text = RightCurrent.ToString();
        }

        protected internal override void LoadFromConfig(in object value)
        {
            Console.WriteLine($">>>>>>>>>>>>>>> DoubleIntegerDisplayType is type {value.GetType()}");
            if (value is (long, long))
            {
                (long, long) tuple = ((long, long))value;
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
