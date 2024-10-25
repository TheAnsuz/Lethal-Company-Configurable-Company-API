using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuButtons : IMenuPart
    {
        private Reference<MenuBind> Bind;

        protected GameObject ButtonSave { get; private set; }
        protected GameObject ButtonReset { get; private set; }
        protected GameObject ButtonRestore { get; private set; }
        protected GameObject ButtonCopy { get; private set; }
        protected GameObject ButtonPaste { get; private set; }

        protected TMP_InputField ButtonRandomize_Input { get; private set; }
        protected TextMeshProUGUI ButtonRandomize_Extra { get; private set; }
        protected Button ButtonRandomize_Clear { get; private set; }
        protected Button ButtonRandomize_Redo { get; private set; }
        protected Button ButtonRandomize_Accept { get; private set; }

        internal MenuButtons(Reference<MenuBind> bind)
        {
            Bind = bind;

            ButtonSave = Bind.Item.Menu.FindChild("Buttons/Save");
            //ButtonSave.AddComponent<NoDrawGraphic>();
            ButtonSave.GetComponent<Button>().onClick.AddListener(OnSave);

            ButtonReset = Bind.Item.Menu.FindChild("Buttons/Reset");
            //ButtonReset.AddComponent<NoDrawGraphic>();
            ButtonReset.GetComponent<Button>().onClick.AddListener(OnReset);

            ButtonRestore = Bind.Item.Menu.FindChild("Buttons/Restore");
            //ButtonRestore.AddComponent<NoDrawGraphic>();
            ButtonRestore.GetComponent<Button>().onClick.AddListener(OnRestore);

            ButtonCopy = Bind.Item.Menu.FindChild("Buttons/Copy");
            //ButtonCopy.AddComponent<NoDrawGraphic>();
            ButtonCopy.GetComponent<Button>().onClick.AddListener(OnCopy);

            ButtonPaste = Bind.Item.Menu.FindChild("Buttons/Paste");
            //ButtonPaste.AddComponent<NoDrawGraphic>();
            ButtonPaste.GetComponent<Button>().onClick.AddListener(OnPaste);

            var randomize = Bind.Item.Menu.FindChild("Buttons/Randomize");
            ButtonRandomize_Extra = randomize.FindChild("Text/Extra").GetComponent<TextMeshProUGUI>();
            ButtonRandomize_Input = randomize.FindChild("Handler/Input").GetComponent<TMP_InputField>();
            ButtonRandomize_Redo = randomize.FindChild("Handler/Redo").GetComponent<Button>();
            ButtonRandomize_Clear = randomize.FindChild("Handler/Clear").GetComponent<Button>();
            ButtonRandomize_Accept = randomize.FindChild("Handler/Accept").GetComponent<Button>();

            ButtonRandomize_Input.characterLimit = 10;
            ButtonRandomize_Input.contentType = TMP_InputField.ContentType.Custom;
            ButtonRandomize_Input.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            ButtonRandomize_Input.inputValidator = CustomCharacterValidator.Create(SeedValidator);

            ButtonRandomize_Extra.text = "";

            ButtonRandomize_Clear.onClick.AddListener(OnRandomize_Clear);
            ButtonRandomize_Redo.onClick.AddListener(OnRandomize_Redo);
            ButtonRandomize_Accept.onClick.AddListener(OnRandomize_Accept);
        }

        private void OnRandomize_Accept()
        {
            if (MenuPopup.IsAdvancedInput)
                OnRandomize_Accept_Action();
            else
                MenuPopup.Show("Randomizer", "Are you sure you want to randomize configurations?\n\nCurrent configurations will be lost\n<color=#c90a0a>WARNING </color>gameplay might not be balanced", OnRandomize_Accept_Action, MenuPopup.NO_ACTION);
        }

        private void OnRandomize_Accept_Action()
        {
            if (RandomSeedParser.IsValidString(ButtonRandomize_Input.text))
            {
                string seedString = RandomSeedParser.FormalizeString(ButtonRandomize_Input.text);
                MenuEventRouter.OnClick_Randomize(seedString);
            }
            else
            {
                ButtonRandomize_Input.text = "";
            }
        }

        private void OnRandomize_Redo()
        {
            ButtonRandomize_Input.text = RandomSeedParser.GenerateRandom();
        }

        private void OnRandomize_Clear()
        {
            ButtonRandomize_Input.text = "";
        }

        private bool SeedValidator(ref string text, ref int pos, ref char ch)
        {
            if (text.Length >= 10)
                return false;

            ch = char.ToUpperInvariant(ch);

            if (RandomSeedParser.IsValidChar(ch))
            {
                pos = text.Length + 1;
                return true;
            }
            return false;
        }

        private void OnSave(/*object sender, PointerEventData e*/)
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_Save();
            else
                MenuPopup.Show("Information", "Are you sure you want to save?\n\nPreviously saved configurations will be overwriten", MenuEventRouter.OnClick_Save, MenuPopup.NO_ACTION);
        }
        private void OnReset(/*object sender, PointerEventData e*/)
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_Reset();
            else
                MenuPopup.Show("Information", "Are you sure you want to reset?\n\nYour saved configurations will be lost and every setting will be set to it's default value", MenuEventRouter.OnClick_Reset, MenuPopup.NO_ACTION);
        }
        private void OnRestore(/*object sender, PointerEventData e*/)
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_Restore();
            else
                MenuPopup.Show("Information", "Are you sure you want to restore?\n\nYour modifications will be lost and settings will be set from the last saved values", MenuEventRouter.OnClick_Restore, MenuPopup.NO_ACTION);
        }
        private void OnCopy(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_Copy();
        }
        private void OnPaste(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_Paste();
        }


        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuButtons deletion in progress");
            Bind = null;
            UnityEngine.Object.Destroy(ButtonSave);
            UnityEngine.Object.Destroy(ButtonReset);
            UnityEngine.Object.Destroy(ButtonRestore);
            UnityEngine.Object.Destroy(ButtonCopy);
            UnityEngine.Object.Destroy(ButtonPaste);

            ButtonSave = null;
            ButtonReset = null;
            ButtonRestore = null;
            ButtonCopy = null;
            ButtonPaste = null;
            ButtonRandomize_Input = null;
            ButtonRandomize_Extra = null;
            ButtonRandomize_Clear = null;
            ButtonRandomize_Redo = null;
            ButtonRandomize_Accept = null;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateContent()
        {
            yield break;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateSelf()
        {
            yield break;
        }

        internal void SetRandomizerDetails(InfoProvider info)
        {
            if (info.IsSpecialSeed)
            {
                ButtonRandomize_Extra.text = "Special Seed";
            }
            else if (info.IsChallenge)
            {
                ButtonRandomize_Extra.text = "Challenge Seed";
            }
            else
            {
                ButtonRandomize_Extra.text = "";
            }

            ButtonRandomize_Input.text = info.SeedString;
        }

#if DEBUG
        ~MenuButtons()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuButtons deleted");
        }
#endif
    }
}
