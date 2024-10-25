using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    public class MenuPreset
    {
        public readonly string File;
        public readonly string Text;
        public GameObject Object { get; private set; }
        public TextMeshProUGUI Label { get; private set; }
        public Button Button { get; private set; }
        public event Action<MenuPreset> OnClick;

        public MenuPreset(string file, GameObject container)
        {
            File = file;
            Object = UnityEngine.Object.Instantiate(MenuPrefabs.Preset, container.transform);
            Label = Object.FindChild("Text").GetComponent<TextMeshProUGUI>();
            Text = Path.GetFileNameWithoutExtension(file);
            Label.text = Text;
            Button = Object.GetComponent<Button>();
            Button.onClick.AddListener(InternalOnClick);
        }

        private void InternalOnClick()
        {
            OnClick?.Invoke(this);
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(Object);
            Object = null;
            Label = null;
            Button = null;
            OnClick = null;
        }

#if DEBUG
        ~MenuPreset()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuPreset deleted");
        }
#endif
    }
}
