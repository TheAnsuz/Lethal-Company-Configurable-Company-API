using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuTag
    {
        public static MenuTag CreateTag(Transform parent)
        {
            return new(Object.Instantiate(MenuPresets.Tag, parent, false));
        }

        public readonly TextMeshProUGUI Text;
        public readonly GameObject Tag;

        internal MenuTag(GameObject tag)
        {
            Tag = tag;
            Text = tag.FindChild("Text").GetComponent<TextMeshProUGUI>();
        }

        public void SetColor(Color color)
        {
            Text.color = color;
        }

        public void SetText(string text)
        {
            Text.SetText(text);
        }

        public void SetVisible(bool visible)
        {
            Tag.SetActive(visible);
        }

        public bool IsVisible() => Tag.activeSelf;
    }
}
