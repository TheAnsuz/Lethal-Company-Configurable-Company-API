using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amrv.ConfigurableCompany.content.display
{
    // Panel containing the configuration display, the tooltip display and buttons
    public sealed class ConfigurationScreen
    {
        public static Color MENU_AREA_BACKGROUND_COLOR = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;

        public const string PANEL_NAME = "ConfigurableCompanyPanel";

        public readonly GameObject ScreenArea;

        public readonly GameObject ConfigurationMenuArea;
        public readonly ConfigurationMenu ConfigurationMenu;

        public readonly GameObject TooltipMenuArea;
        public readonly TooltipMenu TooltipMenu;

        public readonly GameObject ButtonsMenuArea;
        public readonly ButtonsMenu ButtonsMenu;

        public ConfigurationScreen(GameObject parent)
        {
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

            TooltipMenu = new(TooltipMenuArea, this);
            ConfigurationMenu = new(ConfigurationMenuArea, this);
            ButtonsMenu = new(ButtonsMenuArea, this);
        }

        internal void OnEnterConfigurationMenuArea(object sender, PointerEventData e)
        {
            TooltipMenu.DisplayConfig(null);
        }

        public ConfigurationCategoryDisplay AddCategory(ConfigurationCategory category)
        {
            ConfigurationCategoryDisplay cat = new(category);
            ConfigurationMenu.Add(cat);
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

        public void Refresh()
        {
            ConfigurationMenu.RefreshContent();
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
