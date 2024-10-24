using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuToggle : IMenuPart
    {
        protected const string TEXT_SHOW_MENU = "Show menu";
        protected const string TEXT_HIDE_MENU = "Hide menu";

        protected Reference<MenuBind> Bind;

        protected GameObject MenuContainer { get; private set; }
        protected GameObject HandleClosed { get; private set; }
        protected GameObject HandleOpen { get; private set; }
        protected TextMeshProUGUI Text { get; private set; }

        private bool _open = true;
        public bool Open
        {
            get => _open;
            set
            {
                if (_open == value) return;

                HandleClosed.SetActive(!value);
                HandleOpen.SetActive(value);
                Bind.Item.Menu.SetActive(value);
                Bind.Item.Overlay.SetActive(value);
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
                Bind.Item.ShowMenu.SetActive(!value);
                _locked = value;
            }
        }

        internal MenuToggle(Reference<MenuBind> bind, GameObject container)
        {
            Bind = bind;
            MenuContainer = container;

            if (!Bind.Item.ShowMenu.TryGetComponent(out Graphic g))
                Bind.Item.ShowMenu.AddComponent<NoDrawGraphic>();
            Bind.Item.ShowMenu.AddComponent(out RegionButton button);

            button.OnMouseClick += OnClick;

            HandleClosed = bind.Item.ShowMenu.FindChild("HandleArea/Closed");
            HandleOpen = bind.Item.ShowMenu.FindChild("HandleArea/Open");

            Text = bind.Item.ShowMenu.FindChild("TextArea/Text").GetComponent<TextMeshProUGUI>();
        }

        private void OnClick(object sender, PointerEventData e)
        {
            if (!Locked)
                Open = !Open;
        }

        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuToggle deletion in progress");
            Bind = null;

            UnityEngine.Object.Destroy(MenuContainer);
            UnityEngine.Object.Destroy(HandleClosed);
            UnityEngine.Object.Destroy(HandleOpen);

            MenuContainer = null;
            HandleClosed = null;
            HandleOpen = null;
            Text = null;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateContent()
        {
            // Content is updated automatically
            yield break;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateSelf()
        {
            // Self is updated automatically
            yield break;
        }

#if DEBUG
        ~MenuToggle()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuToggle deleted");
        }
#endif
    }
}
