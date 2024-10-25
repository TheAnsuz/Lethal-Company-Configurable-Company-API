using Amrv.ConfigurableCompany.Core.Display.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Amrv.ConfigurableCompany.Plugin;
using TMPro;
using Amrv.ConfigurableCompany.Core.Extensions;
using UnityEngine.InputSystem;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    public class MenuPopup
    {
        public static readonly Action NO_ACTION = () => { };

        private static MenuPopup _instance;

        public static bool IsAdvancedInput => Keyboard.current.shiftKey.isPressed;

        public static void Show(string title, string description, Action onConfirm, Action onCancel)
        {
            GetInstance().Open(title, description, onConfirm, onCancel);
        }

        public static MenuPopup GetInstance()
        {
            return _instance ?? Create();
        }

        protected internal static MenuPopup Create()
        {
            ConfigurableCompanyPlugin.Debug($"Creating Menu Popup [0/2]");

            if (_instance != null)
            {
                return _instance;
            }
            ConfigurableCompanyPlugin.Debug($"Creating Menu Popup [1/2]");

            _instance = new MenuPopup()
            {
                Visible = false
            };

            ConfigurableCompanyPlugin.Debug($"Creating Menu Popup [2/2]");
            return _instance;
        }

        protected static internal void Destroy()
        {
            _instance?.DestroyInstance();
            _instance = null;
        }

        private readonly GameObject _object;
        private readonly TextMeshProUGUI _titleText;
        private readonly TextMeshProUGUI _descriptionText;
        private readonly Button _confirmButton;
        private readonly Button _cancelButton;

        private Action _actionCancel;
        private Action _actionConfirm;

        public string Title
        {
            get => _titleText.text;
            private set => _titleText.SetText(value);
        }

        public string Description
        {
            get => _descriptionText.text;
            private set => _descriptionText.SetText(value);
        }

        public bool Visible
        {
            get => _object.activeSelf;
            private set => _object.SetActive(value);
        }

        public bool CanConfirm
        {
            get => _confirmButton.gameObject.activeSelf;
            private set => _confirmButton?.gameObject.SetActive(value);
        }

        public bool CanCancel
        {
            get => _cancelButton.gameObject.activeSelf;
            private set => _cancelButton?.gameObject.SetActive(value);
        }

        private MenuPopup()
        {
            _object = UnityEngine.Object.Instantiate(MenuPrefabs.Popup, GlobalCanvas.Instance.transform, false);
            _titleText = _object.FindChild("Box/TitleBox/Title").GetComponent<TextMeshProUGUI>();
            _descriptionText = _object.FindChild("Box/DescriptionBox/Description").GetComponent<TextMeshProUGUI>();
            _confirmButton = _object.FindChild("Box/Buttons/Confirm").GetComponent<Button>();
            _cancelButton = _object.FindChild("Box/Buttons/Cancel").GetComponent<Button>();

            _confirmButton.onClick.AddListener(OnConfirm);
            _cancelButton.onClick.AddListener(OnCancel);
        }

        public void Open(string title, string description, Action onConfirm = null, Action onCancel = null)
        {
            if (Visible)
            {
                if (CanCancel)
                    OnCancel();
                else
                    OnConfirm();
            }

            ConfigurableCompanyPlugin.Debug("MenuPopup > Open");

            Title = title;
            Description = description;

            CanCancel = onCancel != null;
            CanConfirm = onConfirm != null || !CanCancel;

            _actionCancel = onCancel;
            _actionConfirm = onConfirm;

            Visible = true;
        }

        private void OnCancel()
        {
            ConfigurableCompanyPlugin.Debug("MenuPopup > OnCancel");
            var action = _actionCancel;
            Close();
            action?.Invoke();
        }

        private void OnConfirm()
        {
            ConfigurableCompanyPlugin.Debug("MenuPopup > OnConfirm");
            var action = _actionConfirm;
            Close();
            action?.Invoke();
        }

        private void Close()
        {
            Title = string.Empty;
            Description = string.Empty;
            CanCancel = false;
            CanConfirm = true;
            _actionConfirm = null;
            _actionCancel = null;
            Visible = false;
        }

        private void DestroyInstance()
        {
            if (_object != null)
                UnityEngine.Object.Destroy(_object);
            _actionConfirm = null;
            _actionCancel = null;
        }
    }
}
