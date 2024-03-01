using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class EnumDisplayType : ConfigDisplay
    {
        public readonly object[] Values;
        private int _index;
        public int CurrentIndex
        {
            get => _index;
            protected set
            {
                if (value < 0 || value >= Values.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                InputItemName.SetText(Values[value].ToString());
                _index = value;
            }
        }

        protected GameObject EnabledObject;
        protected GameObject ToggleObject;

        protected TextMeshProUGUI InputItemName;
        protected TextMeshProUGUI Name;

        protected Button LeftButton;
        protected Button RightButton;

        public EnumDisplayType(params object[] values)
        {
            Values = [.. values];
        }

        public EnumDisplayType(IEnumerable values)
        {
            Values = [.. values];
        }

        protected override GameObject CreateContainer(CConfig config)
        {
            GameObject container = UnityEngine.Object.Instantiate(MenuPresets.Config_Enum);

            EnabledObject = container.FindChild("Buttons/Toggle/Dot");
            Name = container.FindChild("Name").GetComponent<TextMeshProUGUI>();
            InputItemName = container.FindChild("Input/Item/Name").GetComponent<TextMeshProUGUI>();

            LeftButton = container.FindChild("Input/Left").GetComponent<Button>();
            RightButton = container.FindChild("Input/Right").GetComponent<Button>();

            LeftButton.onClick.AddListener(Previous);
            RightButton.onClick.AddListener(Next);

            container.FindChild("Buttons/Restore").GetComponent<Button>().onClick.AddListener(Restore);
            container.FindChild("Buttons/Reset").GetComponent<Button>().onClick.AddListener(Reset);
            container.FindChild("Buttons/Toggle").GetComponent<Button>().onClick.AddListener(Toggle);

            container.FindChild("Buttons/Toggle").SetActive(config.Toggleable);

            Name.SetText(config.Name);
            return container;
        }

        private void Previous()
        {
            if (CurrentIndex > 0)
                CurrentIndex--;
            else
                CurrentIndex = Values.Length - 1;
        }

        private void Next()
        {
            if (CurrentIndex < Values.Length - 1)
                CurrentIndex++;
            else
                CurrentIndex = 0;
        }

        protected internal override void LoadFromConfig(in object value)
        {
            if (value is int vInt)
                CurrentIndex = vInt;
            else
            {
                for (int i = 0; i < Values.Length; i++)
                {
                    if (Values[i].Equals(value))
                    {
                        CurrentIndex = i;
                        return;
                    }
                }
            }
        }

        protected internal override void SaveToConfig(out object value)
        {
            value = CurrentIndex;
        }

        protected internal override void WhenToggled(bool enabled)
        {
            LeftButton.interactable = enabled;
            RightButton.interactable = enabled;
            EnabledObject.SetActive(enabled);
        }
    }
}
