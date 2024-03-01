using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.ConfigTypes
{
    public abstract class DoubleConfigDisplay : ConfigDisplay
    {
        public const int MAX_CHARACTERS = 7;

        protected abstract TMP_InputField.ContentType LeftContentType { get; }
        protected abstract TMP_InputField.ContentType RightContentType { get; }

        protected GameObject EnabledObject;

        protected TMP_InputField RightInput;
        protected TMP_InputField LeftInput;
        protected TextMeshProUGUI Name;

        protected override GameObject CreateContainer(CConfig config)
        {
            GameObject container = UnityEngine.Object.Instantiate(MenuPresets.Config_DoubleInput);

            Name = container.FindChild("Name").GetComponent<TextMeshProUGUI>();
            EnabledObject = container.FindChild("Buttons/Toggle/Dot");

            RightInput = container.FindChild("Input/Right").GetComponent<TMP_InputField>();
            LeftInput = container.FindChild("Input/Left").GetComponent<TMP_InputField>();

            RightInput.contentType = LeftContentType;
            RightInput.characterLimit = MAX_CHARACTERS;
            RightInput.inputValidator = new CustomCharacterValidator(RightValidateChar);
            RightInput.onEndEdit.AddListener(OnRightEditEnd);

            LeftInput.contentType = LeftContentType;
            LeftInput.characterLimit = MAX_CHARACTERS;
            LeftInput.inputValidator = new CustomCharacterValidator(LeftValidateChar);
            LeftInput.onEndEdit.AddListener(OnLeftEditEnd);

            container.FindChild("Buttons/Restore").GetComponent<Button>().onClick.AddListener(Restore);
            container.FindChild("Buttons/Reset").GetComponent<Button>().onClick.AddListener(Reset);
            container.FindChild("Buttons/Toggle").GetComponent<Button>().onClick.AddListener(Toggle);

            container.FindChild("Buttons/Toggle").SetActive(config.Toggleable);

            Name.SetText(config.Name);

            return container;
        }

        protected virtual void OnLeftEditEnd(string text) { }
        protected virtual void OnRightEditEnd(string text) { }
        protected virtual bool RightValidateChar(ref string text, ref int pos, ref char ch) => true;
        protected virtual bool LeftValidateChar(ref string text, ref int pos, ref char ch) => true;

        protected internal override void WhenToggled(bool enabled)
        {
            LeftInput.readOnly = !enabled;
            RightInput.readOnly = !enabled;
            EnabledObject.SetActive(enabled);
        }
    }
}
