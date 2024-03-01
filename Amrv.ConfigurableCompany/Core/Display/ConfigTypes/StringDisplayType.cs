using Amrv.ConfigurableCompany.API;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class StringDisplayType(int length = LargeInputConfigDisplay.MAX_CHARACTERS) : LargeInputConfigDisplay
    {
        protected readonly int MaxLength = length > MAX_CHARACTERS ? MAX_CHARACTERS : length < 1 ? 1 : length;

        protected override TMP_InputField.ContentType ContentType => TMP_InputField.ContentType.Standard;

        protected override GameObject CreateContainer(CConfig config)
        {
            var obj = base.CreateContainer(config);
            InputField.characterLimit = MaxLength;
            return obj;
        }

        protected internal override void LoadFromConfig(in object value)
        {
            InputField.text = value as string;
        }

        protected internal override void SaveToConfig(out object value)
        {
            value = InputField.text;
        }
    }
}
