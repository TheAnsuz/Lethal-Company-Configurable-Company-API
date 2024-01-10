using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using Amrv.ConfigurableCompany.display.component;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    // Panel containing the configuration display, the tooltip display and buttons
    public sealed class ConfigurationScreen
    {
        public static Color MENU_AREA_BACKGROUND_COLOR = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;

        public const string PANEL_NAME = "ConfigurableCompanyPanel";

        public readonly GameObject DisabledObject;
        public readonly GameObject DisabledTitle;

        public readonly GameObject ScreenArea;

        public readonly GameObject ConfigurationMenuArea;
        public readonly ConfigurationMenu ConfigurationMenu;

        public readonly GameObject TooltipMenuArea;
        public readonly TooltipMenu TooltipMenu;

        public readonly GameObject ButtonsMenuArea;
        public readonly ButtonsMenu ButtonsMenu;

        public readonly GameObject PageChangerArea;
        public readonly PageChanger PageChanger;

        public ConfigurationScreen(GameObject parent)
        {
            DisabledObject = UnityObject.Create("Disabled header")
                .SetParent(parent)
                .AddComponent(out RectTransform DisabledObject_Rect)
                .AddComponent(out Image DisabledObject_Image)
                .AddComponent(out Outline DisabledObject_Outline);

            DisabledObject_Rect.anchorMin = new(0, 0);
            DisabledObject_Rect.anchorMax = new(.60f, .12f);
            DisabledObject_Rect.offsetMin = new(0, 0);
            DisabledObject_Rect.offsetMax = new(0, 0);

            DisabledObject_Image.color = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;
            DisabledObject_Outline.effectColor = DisplayUtils.COLOR_VIEWPORT_OUTLINE;

            DisabledTitle = UnityObject.Create("Title")
                .SetParent(DisabledObject_Rect)
                .AddComponent(out RectTransform DisabledTitle_Rect)
                .AddComponent(out RegionButton DisabledTitle_Button)
                .AddComponent(out TextMeshProUGUI DisabledTitle_Text);

            DisabledTitle_Rect.anchorMin = new(0, 0);
            DisabledTitle_Rect.anchorMax = new(1, 1);
            DisabledTitle_Rect.offsetMin = new(0, 0);
            DisabledTitle_Rect.offsetMax = new(0, 0);

            DisabledTitle_Text.margin = new(4, 0, 4, 0);
            DisabledTitle_Text.font = DisplayUtils.GAME_FONT;
            DisabledTitle_Text.fontSize = 10;
            DisabledTitle_Text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            DisabledTitle_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            DisabledTitle_Text.SetText("Configuration's menu has been <color=red>disabled</color> in challenge mode." +
                "\nIf you don't like this change you can vote for it to come back. You have multiple options:" +
                "\n- Vote in the <color=#037bfc><u><link=discord>lethal company modding discord</link></u></color> at the corresponding <color=#8229ff>#mod-releases</color> forum channel." +
                "\n- Contact the developer <color=#037bfc><u><link=developer>the_ansuz</link></u></color> at discord." +
                "\n- Complain about it enough <color=#037bfc><u><link=complain>here</link></u></color>.");

            DisabledTitle_Button.OnMouseClick += delegate (object sender, PointerEventData data)
            {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(DisabledTitle_Text, data.pointerPressRaycast.worldPosition, null);

                if (linkIndex == -1)
                    return;

                string linkId = DisabledTitle_Text.textInfo.linkInfo[linkIndex].GetLinkID();

                switch (linkId)
                {
                    case "discord":
                        Application.OpenURL("https://discord.com/invite/lcmod");
                        break;
                    case "developer":
                        Application.OpenURL("https://discordapp.com/users/341967365908594700");
                        break;
                    case "complain":
                        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                        break;
                }

            };


            DisabledObject.SetActive(false);

            /*
             * Crea un objeto raiz que ocupa todo el area que luego será dividido en sub menus
             */
            ScreenArea = UnityObject.Create(PANEL_NAME)
                .SetParent(parent)
                .AddComponent(out RectTransform ScreenArea_Rect);

            ScreenArea.layer = DisplayUtils.UI_LAYER.value;

            ScreenArea_Rect.anchorMin = new(0, 0);
            ScreenArea_Rect.anchorMax = new(1, 1);
            ScreenArea_Rect.offsetMin = new(0, 0);
            ScreenArea_Rect.offsetMax = new(0, 0);

            /*
             * Crea la zona dedicada al menu de configuraciones
             */
            ConfigurationMenuArea = UnityObject.Create(nameof(ConfigurationMenuArea))
                .SetParent(ScreenArea_Rect)
                .AddComponent(out RectTransform ConfigurationMenuArea_Rect);

            ConfigurationMenuArea_Rect.anchorMin = new(.03f, 0.05f);
            ConfigurationMenuArea_Rect.anchorMax = new(.31f, 0.95f);
            ConfigurationMenuArea_Rect.offsetMin = new(0, 0);
            ConfigurationMenuArea_Rect.offsetMax = new(0, 0);

            TooltipMenuArea = UnityObject.Create(nameof(TooltipMenu))
                .SetParent(ScreenArea_Rect)
                .AddComponent(out RectTransform TooltipMenuArea_Rect);

            TooltipMenuArea_Rect.anchorMin = new(.34f, 0.7f);
            TooltipMenuArea_Rect.anchorMax = new(.8f, 0.95f);
            TooltipMenuArea_Rect.offsetMin = new(0, 0);
            TooltipMenuArea_Rect.offsetMax = new(0, 0);

            ButtonsMenuArea = UnityObject.Create(nameof(ButtonsMenuArea))
                .SetParent(ScreenArea_Rect)
                .AddComponent(out RectTransform ButtonsMenuArea_Rect);

            ButtonsMenuArea_Rect.anchorMin = new(.34f, .05f);
            ButtonsMenuArea_Rect.anchorMax = new(.6f, .15f);
            ButtonsMenuArea_Rect.offsetMin = new(0, 0);
            ButtonsMenuArea_Rect.offsetMax = new(0, 0);

            PageChangerArea = UnityObject.Create(nameof(PageChangerArea))
                .SetParent(ScreenArea_Rect)
                .AddComponent(out Image image)
                .AddComponent(out RectTransform PageChangerArea_Rect);

            PageChangerArea_Rect.anchorMax = new(0.287f, 0.995f);
            PageChangerArea_Rect.anchorMin = new(.03f, 0.96f);
            PageChangerArea_Rect.offsetMin = new(0, 0);
            PageChangerArea_Rect.offsetMax = new(0, 0);

            PageChanger = new(PageChangerArea, this);
            TooltipMenu = new(TooltipMenuArea, this);
            ConfigurationMenu = new(ConfigurationMenuArea, this);
            ButtonsMenu = new(ButtonsMenuArea, this);
        }

        internal void OnEnterConfigurationMenuArea(object sender, PointerEventData e)
        {
            TooltipMenu.DisplayConfig(null);
        }

        public void SetVisible(bool visible)
        {
            ScreenArea.SetActive(visible);
            DisabledObject.SetActive(!visible);
        }

        /*
        public ConfigurationPageDisplay AddPage(ConfigurationPage page)
        {
            return ConfigurationMenu.AddPage(page);
        }

        public void RemovePage(ConfigurationPage page)
        {
            ConfigurationMenu.RemovePage(page);
        }

        public ConfigurationCategoryDisplay AddCategory(ConfigurationCategory category)
        {
            ConfigurationCategoryDisplay cat = new(category);
            ConfigurationMenu.AddCategory(category);
            return cat;
        }

        public void RemoveCategory(ConfigurationCategory category)
        {
            ConfigurationMenu.Remove(category.ID);
        }

        public bool TryGetCategory(ConfigurationCategory category, out ConfigurationCategoryDisplay display) => TryGetCategory(category.ID, out display);
        public bool TryGetCategory(string categoryID, out ConfigurationCategoryDisplay display) => ConfigurationMenu.TryGet(categoryID, out display);
        public bool TryGetCategory(string categoryID, out ConfigurationCategory category)
        {
            if (TryGetCategory(categoryID, out ConfigurationCategoryDisplay display))
            {
                category = display.Category;
                return true;
            }
            category = null;
            return false;
        }

        public ConfigurationItemDisplay AddConfig(Configuration config)
        {
            if (ConfigurationMenu.TryGet(config.Category.ID, out ConfigurationCategoryDisplay categoryDisplay))
            {
                ConfigurationItemDisplay display = config.Type.CreateDisplay(config);
                categoryDisplay.Add(display);
                return display;
            }
            else
            {
                ConfigurableCompanyPlugin.Error($"Tried to create display for config {config.ID} without a category display");
                return null;
            }
        }

        public void RemoveConfig(Configuration config)
        {
            if (ConfigurationMenu.TryGet(config.Category.ID, out ConfigurationCategoryDisplay categoryDisplay))
            {
                categoryDisplay.Remove(config.ID);
            }
        }

        public bool TryGetConfig(Configuration config, out ConfigurationItemDisplay display)
        {
            display = null;
            if (!TryGetCategory(config.Category.ID, out ConfigurationCategoryDisplay found))
            {
                return false;
            }

            if (found.TryGet(config.ID, out ConfigurationItemDisplay categoryDisplay))
            {
                display = categoryDisplay;
                return true;
            }
            return false;
        }

        public bool TryGetConfig(string configID, out ConfigurationItemDisplay display)
        {
            display = null;

            if (!Configuration.TryGet(configID, out Configuration config))
            {
                return false;
            }

            if (!TryGetCategory(config.Category.ID, out ConfigurationCategoryDisplay found))
            {
                return false;
            }

            if (found.TryGet(configID, out ConfigurationItemDisplay categoryDisplay))
            {
                display = categoryDisplay;
                return true;
            }
            return false;
        }

        public bool TryGetConfig(string configID, out Configuration config)
        {
            if (TryGetConfig(configID, out ConfigurationItemDisplay display))
            {
                config = display.Config;
                return true;
            }
            config = null;
            return false;
        }
        */

        public void Refresh()
        {
            ConfigurationMenu.RefreshPages();
            PageChanger.Refresh();
        }

        public void SaveAll()
        {
            ConfigurationMenu.SaveConfigs();
        }

        public void LoadAll()
        {
            ConfigurationMenu.LoadConfigs();
        }
    }
}
