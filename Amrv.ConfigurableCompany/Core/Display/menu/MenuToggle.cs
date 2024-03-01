using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuToggle : IMenuPart
    {
        protected const string TEXT_SHOW_MENU = "Show menu";
        protected const string TEXT_HIDE_MENU = "Hide menu";

        protected readonly MenuBind Bind;

        protected readonly GameObject MenuContainer;
        protected readonly GameObject HandleClosed;
        protected readonly GameObject HandleOpen;
        protected readonly TextMeshProUGUI Text;

        private bool _open = true;
        public bool Open
        {
            get => _open;
            set
            {
                if (_open == value) return;

                HandleClosed.SetActive(!value);
                HandleOpen.SetActive(value);
                Bind.Menu.SetActive(value);
                Bind.Overlay.SetActive(value);
                Text.SetText(value ? TEXT_HIDE_MENU : TEXT_SHOW_MENU);
                _open = value;

                MenuEventRouter.OnClick_ToggleMenu(open: Open);
            }
        }

        private bool _visible = true;
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible == value) return;

                MenuContainer.SetActive(value);
                _visible = value;

                MenuEventRouter.OnAction_VisibleMenu(visible: value);
            }
        }

        private bool _locked = false;
        public bool Locked
        {
            get => _locked;
            set
            {
                if (value)
                {
                    Open = false;
                }
                Bind.ShowMenu.SetActive(!value);
                _locked = value;
            }
        }

        internal MenuToggle(MenuBind bind, GameObject container)
        {
            Bind = bind;
            MenuContainer = container;

            Bind.ShowMenu.AddComponent<NoDrawGraphic>();
            Bind.ShowMenu.AddComponent(out RegionButton button);

            button.OnMouseClick += OnClick;

            HandleClosed = bind.ShowMenu.FindChild("HandleArea/Closed");
            HandleOpen = bind.ShowMenu.FindChild("HandleArea/Open");

            Text = bind.ShowMenu.FindChild("TextArea/Text").GetComponent<TextMeshProUGUI>();
        }

        private void OnClick(object sender, PointerEventData e)
        {
            if (!Locked)
                Open = !Open;
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
