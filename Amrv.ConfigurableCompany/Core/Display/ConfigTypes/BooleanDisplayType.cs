using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public class BooleanDisplayType : ConfigDisplay
    {
        protected GameObject EnabledObject;
        protected GameObject ToggleObject;

        protected GameObject PipOn;
        protected GameObject PipOff;

        protected TextMeshProUGUI Name;
        protected RegionButton RegionButton;

        private bool _active;
        protected bool Active
        {
            get => _active;
            set
            {
                PipOn?.SetActive(value);
                PipOff?.SetActive(!value);

                _active = value;
            }
        }

        protected override GameObject CreateContainer(CConfig config)
        {
            GameObject container = UnityEngine.Object.Instantiate(MenuPresets.Config_Bool);

            EnabledObject = container.FindChild("Buttons/Toggle/Dot");
            Name = container.FindChild("Name").GetComponent<TextMeshProUGUI>();
            ToggleObject = container.FindChild("Input/Toggle");
            PipOn = ToggleObject.FindChild("PipOn");
            PipOff = ToggleObject.FindChild("PipOff");

            ToggleObject.AddComponent<NoDrawGraphic>();
            RegionButton = ToggleObject.AddComponent<RegionButton>();

            RegionButton.onClick.AddListener(Switch);

            container.FindChild("Buttons/Restore").GetComponent<Button>().onClick.AddListener(Restore);
            container.FindChild("Buttons/Reset").GetComponent<Button>().onClick.AddListener(Reset);
            container.FindChild("Buttons/Toggle").GetComponent<Button>().onClick.AddListener(Toggle);

            container.FindChild("Buttons/Toggle").SetActive(config.Toggleable);

            Name.SetText(config.Name);
            return container;
        }

        private void Switch()
        {
            Active = !Active;
        }

        protected internal override void LoadFromConfig(in object value)
        {
            if (value is bool boolean)
                Active = boolean;
        }

        protected internal override void SaveToConfig(out object value)
        {
            value = Active;
        }

        protected internal override void WhenToggled(bool enabled)
        {
            RegionButton.interactable = enabled;
            EnabledObject.SetActive(enabled);
        }
    }
}
