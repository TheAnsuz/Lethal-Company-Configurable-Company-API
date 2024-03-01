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
    public abstract class LargeInputConfigDisplay : ConfigDisplay
    {
        public const int MAX_CHARACTERS = 30;

        protected TMP_InputField InputField;
        protected GameObject EnabledObject;
        protected TextMeshProUGUI Name;

        protected abstract TMP_InputField.ContentType ContentType { get; }
        protected override GameObject CreateContainer(CConfig config)
        {
            GameObject container = UnityEngine.Object.Instantiate(MenuPresets.Config_LargeInput);

            InputField = container.FindChild("Input").GetComponent<TMP_InputField>();
            Name = container.FindChild("Name").GetComponent<TextMeshProUGUI>();
            EnabledObject = container.FindChild("Buttons/Toggle/Dot");

            InputField.contentType = ContentType;
            InputField.characterLimit = MAX_CHARACTERS;
            InputField.inputValidator = new CustomCharacterValidator(ValidateChar);

            InputField.onEndEdit.AddListener(OnEditEnd);

            container.FindChild("Buttons/Restore").GetComponent<Button>().onClick.AddListener(Restore);
            container.FindChild("Buttons/Reset").GetComponent<Button>().onClick.AddListener(Reset);
            container.FindChild("Buttons/Toggle").GetComponent<Button>().onClick.AddListener(Toggle);

            container.FindChild("Buttons/Toggle").SetActive(config.Toggleable);

            Name.SetText(config.Name);

            return container;
        }

        protected virtual void OnEditEnd(string text) { }

        protected virtual bool ValidateChar(ref string text, ref int pos, ref char ch) => true;

        protected internal override void WhenToggled(bool enabled)
        {
            InputField.readOnly = !enabled;
            EnabledObject.SetActive(enabled);
        }
    }
}
