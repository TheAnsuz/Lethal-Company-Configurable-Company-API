using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuSection
    {
        public static MenuSection CreateSection(Transform parent, CSection section)
        {
            return new MenuSection(UnityEngine.Object.Instantiate(MenuPrefabs.Section, parent, false), section);
        }

        private readonly GameObject Container;
        private readonly RectTransform Container_Rect;
        private readonly CSection Section;
        private readonly TextMeshProUGUI Text;
        public readonly GameObject Content;

        private MenuSection(GameObject container, CSection section)
        {
            container.name = $"Section {section.ID}";

            Container = container;
            Container_Rect = Container.GetComponent<RectTransform>();
            Section = section;
            container.FindChild("Name").GetComponent<Button>().onClick.AddListener(OnClick);
            Text = container.FindChild("Name/Text").GetComponent<TextMeshProUGUI>();
            Content = container.FindChild("Content");

            SetName(section.Name);
        }

        private void OnClick()
        {
            Content.SetActive(!Content.activeSelf);
            LayoutRebuilder.ForceRebuildLayoutImmediate(Container_Rect);
        }

        public void SetName(string name)
        {
            Text.SetText(name);
        }
    }
}
