using Amrv.ConfigurableCompany.Core.Config;
using Amrv.ConfigurableCompany.Core.Display.Items;
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

        private void OnClickCreate()
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_PresetCreate(CurrentPresetFile);
            else
                MenuPopup.Show("Config Presets", $"Create preset with name \"{CurrentPresetFile}\"?\nSaved configuration values will be used (make sure you saved your current configs)", () => MenuEventRouter.OnClick_PresetCreate(CurrentPresetFile), MenuPopup.NO_ACTION);
        }
        private void OnClickLoad()
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_PresetLoad(CurrentPresetFile);
            else
                MenuPopup.Show("Config Presets", $"Are you sure you want to load preset \"{CurrentPresetFile}\"?\nYour current settings will be overwriten", () => MenuEventRouter.OnClick_PresetLoad(CurrentPresetFile), MenuPopup.NO_ACTION);
        }
        private void OnClickSave()
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_PresetSave(CurrentPresetFile);
            else
                MenuPopup.Show("Config Presets", $"Are you sure you want to save your current configuration to preset \"{CurrentPresetFile}\"?\nFile will be overwriten", () => MenuEventRouter.OnClick_PresetSave(CurrentPresetFile), MenuPopup.NO_ACTION);
        }
        private void OnClickDelete()
        {
            if (MenuPopup.IsAdvancedInput)
                MenuEventRouter.OnClick_PresetDelete(CurrentPresetFile);
            else
                MenuPopup.Show("Config Presets", $"Are you sure you want to delete \"{CurrentPresetFile}\"?\nYou can't undo this action", () => MenuEventRouter.OnClick_PresetDelete(CurrentPresetFile), MenuPopup.NO_ACTION);
        }

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
            ConfigurableCompanyPlugin.Debug($"MenuPresets > UpdateContent start");

            Dictionary<string, MenuPreset> temp = new(Items);

            foreach (var item in Presets.List)
            {
                if (temp.TryGetValue(item, out var _))
                {
                    temp.Remove(item);
                }
                else
                {
                    AddItem(item);
                }
            }

            foreach (var expired in temp)
                DeleteItem(expired.Key);

            ConfigurableCompanyPlugin.Debug($"MenuPresets > UpdateContent | deletions: {temp.Count} | total: {Presets.List.Count}");
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
