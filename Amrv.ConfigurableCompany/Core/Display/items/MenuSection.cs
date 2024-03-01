using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuSection
    {
        public static MenuSection CreateSection(Transform parent, CSection section)
        {
            return new MenuSection(UnityEngine.Object.Instantiate(MenuPresets.Section, parent, false), section);
        }

        private readonly GameObject Container;
        private readonly CSection Section;
        private readonly TextMeshProUGUI Text;
        public readonly GameObject Content;

        private MenuSection(GameObject container, CSection section)
        {
            container.name = $"Section {section.ID}";

            Container = container;
            Section = section;
            Text = container.FindChild("Name/Text").GetComponent<TextMeshProUGUI>();
            Content = container.FindChild("Content");

            SetName(section.Name);
        }

        public void SetName(string name)
        {
            Text.SetText(name);
        }
    }
}
