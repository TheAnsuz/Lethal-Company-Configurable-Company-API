using Amrv.ConfigurableCompany.Core.Extensions;
using System;
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
        public void UpdateContent()
        {

        }

        [Obsolete("Does nothing on this class")]
        public void UpdateSelf()
        {

        }
    }
}
