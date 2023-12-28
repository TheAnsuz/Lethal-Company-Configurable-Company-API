using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using Amrv.ConfigurableCompany.display.component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace Amrv.ConfigurableCompany.content.display.customObject
{
    public sealed class MenuButton
    {
        public static readonly Color COLOR_TRANSPARENT = new Color(0, 0, 0, 0);

        public readonly GameObject Container;
        public readonly GameObject Area;
        private readonly GameObject AreaButton;
        private readonly VerticalLayoutGroup Area_Layout;

        public readonly GameObject Background;
        public readonly Image BackgroundImage;
        public readonly Outline BackgroundOutline;

        public readonly GameObject Title;
        public readonly TextMeshProUGUI TitleText;

        public readonly RectTransform Transform;

        public readonly RegionButton Button;

        public ButtonClickedEvent OnClick => Button.onClick;

        public TextAnchor Alignment
        {
            get => Area_Layout.childAlignment;
            set => Area_Layout.childAlignment = value;
        }
        public Color TextColor
        {
            get => TitleText.color;
            set => TitleText.color = value;
        }
        public Color BackgroundColor
        {
            get => BackgroundImage.color;
            set => BackgroundImage.color = value;
        }
        public Color OutlineColor
        {
            get => BackgroundOutline.effectColor;
            set => BackgroundOutline.effectColor = value;
        }
        public Vector2 OutlineSize
        {
            get => BackgroundOutline.effectDistance;
            set => BackgroundOutline.effectDistance = value;
        }
        public float FontSize
        {
            get => TitleText.fontSize;
            set => TitleText.fontSize = value;
        }
        public string Text
        {
            get => TitleText.text;
            set => TitleText.SetText(value);
        }

        public MenuButton(string name, GameObject parent)
        {
            Container = UnityObject.Create(name)
                .SetParent(parent)
                .AddComponent(out Transform)
                .AddComponent(out VerticalLayoutGroup Container_Layout)
                .AddComponent(out ContentSizeFitter Container_Fitter);

            Container_Layout.childControlHeight = true;
            Container_Layout.childControlWidth = true;
            Container_Layout.childForceExpandHeight = false;
            Container_Layout.childForceExpandWidth = false;

            Container_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Area = UnityObject.Create("Area")
                .SetParent(Container)
                .AddComponent(out RectTransform Area_Rect)
                .AddComponent(out Button)
                .AddComponent(out Area_Layout);

            Area_Rect.offsetMin = new(0, 0);
            Area_Rect.offsetMax = new(0, 0);

            Area_Layout.childControlHeight = true;
            Area_Layout.childControlWidth = true;
            Area_Layout.childForceExpandHeight = false;
            Area_Layout.childForceExpandWidth = false;
            Area_Layout.childAlignment = TextAnchor.MiddleLeft;

            /*
            AreaButton = UnityObject.Create("Button")
                .SetParent(Area_Rect)
                .AddComponent(out RectTransform AreaButton_Rect)
                .AddComponent(out LayoutElement AreaButton_Element)
                .AddComponent(out Image AreaButton_Image)
                .AddComponent(out Button);

            AreaButton_Element.ignoreLayout = true;
            AreaButton_Rect.anchorMin = new(0, 0);
            AreaButton_Rect.anchorMax = new(1, 1);
            AreaButton_Rect.offsetMax = new(0, 0);
            AreaButton_Rect.offsetMin = new(0, 0);
            AreaButton_Image.color = COLOR_TRANSPARENT;
            */
            Button.colors = DisplayUtils.COLOR_TINT_DEFAULT;
            Button.navigation = DisplayUtils.NO_NAVIGATION;

            Background = UnityObject.Create("Background")
                .SetParent(Area_Rect)
                .AddComponent(out RectTransform Background_Rect)
                .AddComponent(out LayoutElement Background_Element)
                .AddComponent(out BackgroundOutline)
                .AddComponent(out BackgroundImage);

            Background_Element.ignoreLayout = true;
            Background_Rect.anchorMin = new(0, 0);
            Background_Rect.anchorMax = new(1, 1);
            Background_Rect.offsetMax = new(0, 0);
            Background_Rect.offsetMin = new(0, 0);

            Title = UnityObject.Create("Text")
                .SetParent(Area_Rect)
                .AddComponent(out TitleText);

            TitleText.raycastTarget = false;
            TitleText.margin = new(2, 1, 2, 1);
            TitleText.font = DisplayUtils.GAME_FONT;
            TitleText.enableWordWrapping = false;
            TitleText.overflowMode = TextOverflowModes.Truncate;
        }

        public static implicit operator GameObject(MenuButton button)
        {
            return button.Container;
        }

        public static implicit operator RectTransform(MenuButton button)
        {
            return button.Transform;
        }
    }
}
