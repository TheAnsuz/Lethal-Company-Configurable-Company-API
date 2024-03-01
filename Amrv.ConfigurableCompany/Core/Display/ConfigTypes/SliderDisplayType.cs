using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class SliderDisplayType : ConfigDisplay
    {
        protected readonly bool Decimal;
        protected TMP_InputField InputField;
        protected GameObject EnabledObject;
        protected TextMeshProUGUI Name;
        protected Slider SliderField;

        protected float Min;
        protected float Max;

        public SliderDisplayType(float min, float max)
        {
            Decimal = true;
            Min = min;
            Max = max;
        }
        public SliderDisplayType(int min, int max)
        {
            Decimal = false;
            Min = min;
            Max = max;
        }

        protected override GameObject CreateContainer(CConfig config)
        {
            GameObject container = UnityEngine.Object.Instantiate(MenuPresets.Config_Slider);

            InputField = container.FindChild("Value").GetComponent<TMP_InputField>();
            Name = container.FindChild("Name").GetComponent<TextMeshProUGUI>();
            EnabledObject = container.FindChild("Buttons/Toggle/Dot");

            InputField.contentType = Decimal ? TMP_InputField.ContentType.DecimalNumber : TMP_InputField.ContentType.IntegerNumber;

            InputField.onEndEdit.AddListener(OnEditEnd);

            SliderField = container.FindChild("Input").GetComponent<Slider>();

            SliderField.wholeNumbers = !Decimal;
            SliderField.minValue = Min;
            SliderField.maxValue = Max;

            SliderField.onValueChanged.AddListener(OnSliderChange);
            InputField.text = $"{SliderField.value:0.###}";

            container.FindChild("Buttons/Restore").GetComponent<Button>().onClick.AddListener(Restore);
            container.FindChild("Buttons/Reset").GetComponent<Button>().onClick.AddListener(Reset);
            container.FindChild("Buttons/Toggle").GetComponent<Button>().onClick.AddListener(Toggle);

            container.FindChild("Buttons/Toggle").SetActive(config.Toggleable);

            Name.SetText(config.Name);

            return container;
        }

        private void OnSliderChange(float value)
        {
            InputField.text = $"{SliderField.value:0.###}";
        }

        private void OnEditEnd(string text)
        {
            if (float.TryParse(text, out float value))
            {
                SliderField.value = value;
            }
            else
            {
                InputField.text = $"{SliderField.value:0.###}";
            }
        }

        protected internal override void LoadFromConfig(in object value)
        {
            if (NumberUtils.IsNumber(value))
            {
                SliderField.value = (float)Convert.ChangeType(value, TypeCode.Single);
            }
        }

        protected internal override void SaveToConfig(out object value)
        {
            value = SliderField.value;
        }

        protected internal override void WhenToggled(bool enabled)
        {
            InputField.readOnly = !enabled;
            SliderField.interactable = enabled;
            EnabledObject.SetActive(enabled);
        }
    }
}
