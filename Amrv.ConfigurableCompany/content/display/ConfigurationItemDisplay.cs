using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Utils.Unity;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    [Obsolete("Use ConfigDisplay")]
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
        public static readonly Color BORDER_COLOR = Color.white;
        public const float FONT_SIZE_MIN = 8;
        public const float FONT_SIZE_NORMAL = 10;
        public const float FONT_SIZE_MAX = 16;
        public static readonly Color COLOR_TRANSPARENT = new(1, 1, 1, 0f);
        public static readonly Color COLOR_INPUT_NORMAL = Color.white;
        public static readonly Color COLOR_INPUT_NORMAL_INVERTED = Color.white;
        public static readonly Color COLOR_INPUT_REQUIRE_RESTART = Color.white;
        public static readonly Color COLOR_INPUT_REQUIRE_RESTART_INVERTED = Color.white;

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

            Container_Button.OnMouseClick += OnClick;
            Container_Button.OnMouseEnter += OnMouseEnter;
            Container_Button.OnMouseExit += OnMouseExit;
            Container_Rect.pivot = new(0, 1);

            //Container_Element.minHeight = Height;
            Container_Element.preferredHeight = Height;
            Container_Element.layoutPriority = 2;
            Container_Element.flexibleHeight = 0;
        }

        protected virtual void OnMouseExit(object sender, PointerEventData e)
        {

        }

        protected virtual void OnMouseEnter(object sender, PointerEventData e)
        {

        }

        protected internal virtual void Delete()
        {
            UnityEngine.Object.Destroy(Container);
        }

        public void SetToConfig() => SetToConfig(Config);
        public void GetFromConfig() => GetFromConfig(Config);

        public abstract void RefreshDisplay();
        protected virtual void OnClick(object sender, PointerEventData e)
        {
            if (Keyboard.current.shiftKey.isPressed)
            {
                if (!Keyboard.current.ctrlKey.isPressed)
                    Config.Reset(model.data.ChangeReason.USER_RESET);

                GetFromConfig();
            }
        }
        protected abstract void GetFromConfig(Configuration Config);
        protected abstract void SetToConfig(Configuration Config);
    }
}
