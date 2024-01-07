using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using Amrv.ConfigurableCompany.display.component;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public abstract class ConfigurationItemDisplay
    {
        public readonly Configuration Config;
        public readonly GameObject Container;
        protected readonly RegionButton Container_Button;
        protected readonly RectTransform Container_Rect;
        protected readonly LayoutElement Container_Element;
        protected readonly Image Container_Image;

        public static readonly float BORDER_PADDING = 2;
        public static readonly float BORDER_SIZE = 1.2f;
        public static readonly Color BORDER_COLOR = DisplayUtils.COLOR_LABEL_TEXT;
        public const float FONT_SIZE_MIN = 8;
        public const float FONT_SIZE_NORMAL = 10;
        public const float FONT_SIZE_MAX = 16;
        public static readonly Color COLOR_TRANSPARENT = new(1, 1, 1, 0f);
        public static readonly Color COLOR_INPUT_NORMAL = DisplayUtils.COLOR_INPUT_TEXT;
        public static readonly Color COLOR_INPUT_NORMAL_INVERTED = DisplayUtils.COLOR_INPUT_TEXT;
        public static readonly Color COLOR_INPUT_REQUIRE_RESTART = DisplayUtils.COLOR_INPUT_TEXT_REQUIRE_RESTART;
        public static readonly Color COLOR_INPUT_REQUIRE_RESTART_INVERTED = DisplayUtils.COLOR_INPUT_TEXT_REQUIRE_RESTART;

        public ConfigurationMenu OwnerMenu { get; protected set; }

        public abstract int Height { get; }

        protected ConfigurationItemDisplay(Configuration Config)
        {
            this.Config = Config;

            Container = UnityObject.Create(Config.ID)
                .AddComponent(out Container_Button)
                .AddComponent(out Container_Element)
                .AddComponent(out Container_Image)
                .AddComponent(out Container_Rect);

            Container_Image.color = COLOR_TRANSPARENT;

            Container_Button.onClick.AddListener(OnClick);
            Container_Button.OnMouseEnter += OnMouseEnter;
            Container_Button.OnMouseExit += OnMouseExit;
            Container_Button.colors = DisplayUtils.COLOR_TINT_DEFAULT;
            Container_Button.navigation = DisplayUtils.NO_NAVIGATION;
            Container_Rect.pivot = new(0, 1);

            //Container_Element.minHeight = Height;
            Container_Element.preferredHeight = Height;
            Container_Element.layoutPriority = 2;
            Container_Element.flexibleHeight = 0;
        }

        protected virtual void OnMouseExit(object sender, PointerEventData e)
        {
            if (OwnerMenu == null) return;

            OwnerMenu.Screen?.TooltipMenu?.DisplayConfig(null);
        }

        protected virtual void OnMouseEnter(object sender, PointerEventData e)
        {
            if (OwnerMenu == null) return;

            OwnerMenu.Screen?.TooltipMenu?.DisplayConfig(Config);
        }

        public void AddToParent(ConfigurationCategoryDisplay categoryDisplay, GameObject container)
        {
            OwnerMenu = categoryDisplay.OwnerMenu;
            Container.transform.SetParent(container.transform, false);
            OnAdd(categoryDisplay, container);
        }

        public void RemoveFromParent(ConfigurationCategoryDisplay categoryDisplay, GameObject container)
        {
            OwnerMenu = null;
            Container.transform.parent = null;
            OnRemove(categoryDisplay, container);
        }

        protected internal virtual void Delete()
        {
            UnityEngine.Object.Destroy(Container);
        }

        public void SetToConfig() => SetToConfig(Config);
        public void GetFromConfig() => GetFromConfig(Config);

        public abstract void RefreshConfig();
        protected abstract void OnClick();
        protected abstract void GetFromConfig(Configuration Config);
        protected abstract void SetToConfig(Configuration Config);
        protected virtual void OnAdd(ConfigurationCategoryDisplay category, GameObject parent) { }
        protected virtual void OnRemove(ConfigurationCategoryDisplay category, GameObject parent) { }
    }
}
