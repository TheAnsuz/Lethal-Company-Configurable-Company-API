using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuButtons : IMenuPart
    {
        private readonly MenuBind Bind;

        protected readonly GameObject ButtonSave;
        protected readonly GameObject ButtonReset;
        protected readonly GameObject ButtonRestore;
        protected readonly GameObject ButtonCopy;
        protected readonly GameObject ButtonPaste;

        protected readonly TMP_InputField ButtonRandomize_Input;
        protected readonly TextMeshProUGUI ButtonRandomize_Extra;
        protected readonly Button ButtonRandomize_Clear;
        protected readonly Button ButtonRandomize_Redo;
        protected readonly Button ButtonRandomize_Accept;

        internal MenuButtons(MenuBind bind)
        {
            Bind = bind;

            ButtonSave = Bind.Menu.FindChild("Buttons/Save");
            //ButtonSave.AddComponent<NoDrawGraphic>();
            ButtonSave.GetComponent<Button>().onClick.AddListener(OnSave);

            ButtonReset = Bind.Menu.FindChild("Buttons/Reset");
            //ButtonReset.AddComponent<NoDrawGraphic>();
            ButtonReset.GetComponent<Button>().onClick.AddListener(OnReset);

            ButtonRestore = Bind.Menu.FindChild("Buttons/Restore");
            //ButtonRestore.AddComponent<NoDrawGraphic>();
            ButtonRestore.GetComponent<Button>().onClick.AddListener(OnRestore);

            ButtonCopy = Bind.Menu.FindChild("Buttons/Copy");
            //ButtonCopy.AddComponent<NoDrawGraphic>();
            ButtonCopy.GetComponent<Button>().onClick.AddListener(OnCopy);

            ButtonPaste = Bind.Menu.FindChild("Buttons/Paste");
            //ButtonPaste.AddComponent<NoDrawGraphic>();
            ButtonPaste.GetComponent<Button>().onClick.AddListener(OnPaste);

            var randomize = Bind.Menu.FindChild("Buttons/Randomize");
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
            MenuEventRouter.OnClick_Save();
        }
        private void OnReset(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_Reset();
        }
        private void OnRestore(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_Restore();
        }
        private void OnCopy(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_Copy();
        }
        private void OnPaste(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_Paste();
        }


        [Obsolete("Does nothing on this class")]
        public void Destroy()
        {
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
    }
}
