using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Extensions;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuTooltip : IMenuPart
    {
        private readonly MenuBind Bind;

        private readonly GameObject Container;

        protected readonly GameObject Headline;
        protected readonly TextMeshProUGUI Headline_Text;
        protected readonly GameObject Information;
        protected readonly TextMeshProUGUI Information_Text;
        protected readonly GameObject Tags;
        protected readonly GameObject TagsPanel;

        protected readonly MenuTag Tag_Experimental;
        protected readonly MenuTag Tag_Type;
        protected readonly MenuTag Tag_Default;
        protected readonly MenuTag Tag_Synchronized;

        internal MenuTooltip(MenuBind bind)
        {
            Bind = bind;

            Container = Bind.Menu.FindChild("Tooltip");

            Headline = Container.FindChild("Headline");
            Information = Container.FindChild("Information");
            Tags = Container.FindChild("Tags");
            TagsPanel = Container.FindChild("Tags/Tags panel");

            Headline_Text = Container.FindChild("Headline/Text panel/Text").GetComponent<TextMeshProUGUI>();
            Headline_Text.SetText("");

            Information_Text = Container.FindChild("Information/Body/Horizontal panel/Text").GetComponent<TextMeshProUGUI>();
            Information_Text.SetText("");
            Information_Text.maxVisibleLines = 11;

            Tag_Experimental = MenuTag.CreateTag(TagsPanel.transform);
            Tag_Experimental.SetText("Experimental");
            Tag_Experimental.SetColor(new Color32(219, 106, 68, 255));
            Tag_Experimental.SetVisible(false);

            Tag_Type = MenuTag.CreateTag(TagsPanel.transform);
            Tag_Type.SetColor(new Color32(144, 196, 71, 255));
            Tag_Type.SetText(null);

            Tag_Default = MenuTag.CreateTag(TagsPanel.transform);
            Tag_Default.SetColor(new Color32(171, 171, 171, 255));
            Tag_Default.SetText(null);

            Tag_Synchronized = MenuTag.CreateTag(TagsPanel.transform);
            Tag_Synchronized.SetText("Synchronize with client");
            Tag_Synchronized.SetVisible(false);
            Tag_Synchronized.SetColor(new Color32(39, 214, 214, 255));

            DisplayedConfig = null;
        }

        private CConfig _displayedConfig;
        public CConfig DisplayedConfig
        {
            get => _displayedConfig;
            set
            {
                if (_displayedConfig == value) return;

                _displayedConfig = value;
                if (value == null)
                {
                    Container.SetActive(false);
                    return;
                }

                Information_Text.SetText(value.Tooltip);
                Headline_Text.SetText(value.Name);
                Tag_Type.SetText(value.Type.TypeName);
                Tag_Default.SetText("Default: " + value.Default.ToString());
                Container.SetActive(true);
                Tag_Synchronized.SetVisible(value.Synchronized);
                Tag_Experimental.SetVisible(value.Experimental);
            }
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(Headline);
            UnityEngine.Object.Destroy(Information);
            UnityEngine.Object.Destroy(Tags);
            UnityEngine.Object.Destroy(TagsPanel);
        }

        public void UpdateContent()
        {

        }

        public void UpdateSelf()
        {

        }
    }
}
