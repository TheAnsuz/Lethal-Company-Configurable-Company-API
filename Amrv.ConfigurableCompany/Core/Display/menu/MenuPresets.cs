using Amrv.ConfigurableCompany.Core.Config;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    public class MenuPresets : IMenuPart
    {
        private GameObject Container;
        private TMP_InputField InputField;
        private GameObject Content;

        private string CurrentPresetFile;
        private Dictionary<string, MenuPreset> Items = [];

        internal MenuPresets(Reference<MenuBind> menuBind)
        {
            Container = menuBind.Item.Menu.FindChild("Presets");
            InputField = Container.FindChild("Input").GetComponent<TMP_InputField>();

            Content = Container.FindChild("List/Viewport/Content");

            Container.FindChild("Buttons/Create").GetComponent<Button>().onClick.AddListener(OnClickCreate);
            Container.FindChild("Buttons/Load").GetComponent<Button>().onClick.AddListener(OnClickLoad);
            Container.FindChild("Buttons/Save").GetComponent<Button>().onClick.AddListener(OnClickSave);
            Container.FindChild("Buttons/Delete").GetComponent<Button>().onClick.AddListener(OnClickDelete);

            InputField.onEndEdit.AddListener(UpdateCurrentFile);
        }

        private void UpdateCurrentFile(string text)
        {
            CurrentPresetFile = text + ".ccfg";
        }

        private void OnClickCreate() => MenuEventRouter.OnClick_PresetCreate(CurrentPresetFile);
        private void OnClickLoad() => MenuEventRouter.OnClick_PresetLoad(CurrentPresetFile);
        private void OnClickSave() => MenuEventRouter.OnClick_PresetSave(CurrentPresetFile);
        private void OnClickDelete() => MenuEventRouter.OnClick_PresetDelete(CurrentPresetFile);

        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuPresets deletion in progress ({Items.Count} presets)");
            foreach (MenuPreset preset in Items.Values)
            {
                preset.Destroy();
            }
            Items.Clear();

            Items = null;

            Object.Destroy(Content);
            Object.Destroy(Container);

            Container = null;
            InputField = null;
            Content = null;
        }

        public IEnumerator UpdateContent()
        {
            Dictionary<string, MenuPreset> temp = new(Items);

            foreach (var item in Presets.List)
            {
                if (temp.TryGetValue(item, out var _))
                    temp.Remove(item);
                else
                    AddItem(item);
            }

            foreach (var expired in temp)
                DeleteItem(expired.Key);

            /*
            if (Items.Count > 0)
            {
                var enumerator = Items.GetEnumerator();
                for (int i = 0; i < Items.Count; i++)
                    enumerator.MoveNext();
                var first = enumerator.Current.Value;

                CurrentPresetFile = first.File;
                InputField.text = first.Text;
            }
            else
            {
                CurrentPresetFile = null;
                InputField.text = null;
            }
            */
            CurrentPresetFile = null;
            InputField.text = null;

            yield break;
        }

        private void OnPrefabClick(MenuPreset preset)
        {
            CurrentPresetFile = preset.File;
            InputField.text = preset.Text;
        }

        public void DeleteItem(string item)
        {
            if (Items.TryGetValue(item, out var obj))
            {
                obj.Destroy();
                Items.Remove(item);
            }
        }

        public void AddItem(string item)
        {
            var preset = new MenuPreset(item, Content);
            Items.Add(item, preset);
            preset.OnClick += OnPrefabClick;
        }

        public IEnumerator UpdateSelf()
        {
            InputField.text = Path.GetFileNameWithoutExtension(CurrentPresetFile);
            yield break;
        }

#if DEBUG
        ~MenuPresets()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuPresets deleted");
        }
#endif
    }
}
