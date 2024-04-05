using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.items
{
    public class MenuPreset
    {
        public readonly string File;
        public readonly string Text;
        public readonly GameObject Object;
        public readonly TextMeshProUGUI Label;
        public readonly Button Button;
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

        public void Delete()
        {
            UnityEngine.Object.Destroy(Object);
        }
    }
}
