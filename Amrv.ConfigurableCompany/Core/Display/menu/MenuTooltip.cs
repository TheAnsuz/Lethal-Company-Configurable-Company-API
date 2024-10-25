using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuTooltip : IMenuPart
    {
        private Reference<MenuBind> Bind;

        private GameObject Container;

        protected GameObject Headline;
        protected TextMeshProUGUI Headline_Text;
        protected GameObject Information;
        protected TextMeshProUGUI Information_Text;
        protected GameObject Tags;
        protected GameObject TagsPanel;

        protected MenuTag Tag_Experimental;
        protected MenuTag Tag_Type;
        protected MenuTag Tag_Default;
        protected MenuTag Tag_Synchronized;
        protected MenuTag Tag_Randomizable;

        internal MenuTooltip(Reference<MenuBind> bind)
        {
            Bind = bind;

            Container = Bind.Item.Menu.FindChild("Tooltip");

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

            Tag_Randomizable = MenuTag.CreateTag(TagsPanel.transform);
            Tag_Randomizable.SetText("Randomizable");
            Tag_Randomizable.SetVisible(false);
            Tag_Randomizable.SetColor(new Color32(224, 52, 13, 255));

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
                Tag_Randomizable.SetVisible(value.Randomizer.Active);
            }
        }

        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuTooltip deletion in progress");

            Bind = null;

            UnityEngine.Object.Destroy(Container);
            UnityEngine.Object.Destroy(Headline);
            UnityEngine.Object.Destroy(Information);
            UnityEngine.Object.Destroy(Tags);
            UnityEngine.Object.Destroy(TagsPanel);

            Tag_Experimental.Destroy();
            Tag_Type.Destroy();
            Tag_Default.Destroy();
            Tag_Synchronized.Destroy();
            Tag_Randomizable.Destroy();

            Container = null;
            Headline = null;
            Headline_Text = null;
            Information = null;
            Information_Text = null;
            Tags = null;
            TagsPanel = null;
            Tag_Experimental = null;
            Tag_Type = null;
            Tag_Default = null;
            Tag_Synchronized = null;
            Tag_Randomizable = null;

            _displayedConfig = null;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateContent()
        {
            yield break;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateSelf()
        {
            yield break;
        }

#if DEBUG
        ~MenuTooltip()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuTooltip deleted");
        }
#endif
    }
}
