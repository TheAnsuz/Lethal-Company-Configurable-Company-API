using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Core.IO;
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
        private readonly GameObject State_Open;
        private readonly GameObject State_Closed;

        private MenuSection(GameObject container, CSection section)
        {
            container.name = $"Section {section.ID}";

            Container = container;
            Container_Rect = Container.GetComponent<RectTransform>();
            _ = section;
            container.FindChild("Name").GetComponent<Button>().onClick.AddListener(OnClick);
            Text = container.FindChild("Name/Text").GetComponent<TextMeshProUGUI>();
            Content = container.FindChild("Content");

            State_Closed = container.FindChild("Name/State_Closed");
            State_Open = container.FindChild("Name/State_Open");

            SetName(section.Name);

            Section = section;

            SetVisible(IOController.GetSectionOpenState(section));
        }

        private void OnClick()
        {
            SetVisible(!IsVisible());
            MenuEventRouter.OnAction_ToggleSection(Section, IsVisible());
        }

        public bool IsVisible() => Content.activeSelf;

        public void SetVisible(bool visible)
        {
            Content.SetActive(visible);
            State_Open.SetActive(visible);
            State_Closed.SetActive(!visible);
            LayoutRebuilder.ForceRebuildLayoutImmediate(Container_Rect);
        }

        public void SetName(string name)
        {
            Text.SetText(name);
        }
    }
}
