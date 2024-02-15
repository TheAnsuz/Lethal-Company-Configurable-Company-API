using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class TooltipMenu
    {
        public static readonly Color COLOR_BACKGROUND = DisplayUtils.COLOR_TOOLTIP_BACKGROUND;
        public static readonly Color COLOR_BACKGROUND_OUTLINE = DisplayUtils.COLOR_VIEWPORT_OUTLINE;
        public static readonly int TITLE_SIZE = 12;
        public static readonly int TOOLTIP_SIZE = 10;
        public static readonly int TAG_SIZE = 8;
        public static readonly Color COLOR_TITLE = DisplayUtils.COLOR_TOOLTIP_TEXT;
        public static readonly Color COLOR_TEXT = new(1, 1, 1, 1);

        public readonly ConfigurationScreen Screen;

        public readonly GameObject Container;

        public readonly GameObject Header;
        public readonly GameObject HeaderArea;
        public readonly GameObject HeaderTitle;
        public readonly GameObject HeaderBack;
        public readonly TextMeshProUGUI HeaderTitle_Text;

        public readonly GameObject Body;
        public readonly GameObject BodyArea;
        public readonly GameObject BodyTooltip;
        public readonly GameObject BodyBack;
        public readonly TextMeshProUGUI BodyTooltip_Text;
        public readonly LayoutElement BodyTooltip_Element;

        public readonly GameObject Footer;
        public readonly GameObject FooterArea;

        protected readonly GameObject Tag_Synchronized;
        protected readonly GameObject Tag_Type;
        protected readonly GameObject Tag_DefaultValue;
        protected readonly GameObject Tag_NeedsRestart;
        protected readonly GameObject Tag_Experimental;
        protected readonly TextMeshProUGUI Tag_Type_Text;
        protected readonly TextMeshProUGUI Tag_DefaultValue_Text;

        protected internal TooltipMenu(GameObject parent, ConfigurationScreen owner)
        {
            Screen = owner;
            parent.AddComponent<RectMask2D>();

            Container = UnityObject.Create("Display area")
                .SetParent(parent)
                .AddComponent(out RectTransform Container_Rect)
                .AddComponent(out VerticalLayoutGroup Container_Layout);

            Container_Rect.anchorMin = new(0, 0);
            Container_Rect.anchorMax = new(1, 1);
            Container_Rect.offsetMin = new(0, 0);
            Container_Rect.offsetMax = new(0, 0);

            Container_Layout.padding = new(2, 2, 2, 2);
            Container_Layout.childControlHeight = true;
            Container_Layout.childControlWidth = true;
            Container_Layout.childForceExpandHeight = false;
            Container_Layout.childForceExpandWidth = true;
            Container_Layout.spacing = 5;

            // Header
            {
                Header = UnityObject.Create("Header")
                    .SetParent(Container_Rect)
                    .AddComponent(out RectTransform Header_Rect)
                    .AddComponent(out VerticalLayoutGroup Header_Layout)
                    .AddComponent(out ContentSizeFitter Header_Fitter);

                Header_Layout.childControlHeight = true;
                Header_Layout.childControlWidth = true;
                Header_Layout.childForceExpandHeight = false;
                Header_Layout.childForceExpandWidth = false;

                Header_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                HeaderArea = UnityObject.Create("Area")
                    .SetParent(Header_Rect)
                    .AddComponent(out RectTransform HeaderArea_Rect)
                    .AddComponent(out VerticalLayoutGroup HeaderArea_Layout);

                HeaderArea_Rect.offsetMin = new(0, 0);
                HeaderArea_Rect.offsetMax = new(0, 0);

                HeaderArea_Layout.childControlHeight = true;
                HeaderArea_Layout.childControlWidth = true;
                HeaderArea_Layout.childForceExpandHeight = false;
                HeaderArea_Layout.childForceExpandWidth = false;
                HeaderArea_Layout.childAlignment = TextAnchor.MiddleLeft;


                HeaderBack = UnityObject.Create("Background")
                    .SetParent(HeaderArea_Rect)
                    .AddComponent(out RectTransform HeaderBack_Rect)
                    .AddComponent(out LayoutElement HeaderBack_Element)
                    .AddComponent(out Outline HeaderBack_Outline)
                    .AddComponent(out Image HeaderBack_Image);

                HeaderBack_Element.ignoreLayout = true;
                HeaderBack_Rect.anchorMin = new(0, 0);
                HeaderBack_Rect.anchorMax = new(1, 1);
                HeaderBack_Rect.offsetMax = new(0, 0);
                HeaderBack_Rect.offsetMin = new(0, 0);

                HeaderBack_Image.color = COLOR_BACKGROUND;

                HeaderBack_Outline.effectDistance = new(1, -1);
                HeaderBack_Outline.effectColor = COLOR_BACKGROUND_OUTLINE;

                HeaderTitle = UnityObject.Create("Text")
                    .SetParent(HeaderArea_Rect)
                    .AddComponent(out HeaderTitle_Text);

                HeaderTitle_Text.font = DisplayUtils.GAME_FONT;
                HeaderTitle_Text.fontSize = TITLE_SIZE;
                HeaderTitle_Text.color = COLOR_TITLE;
                HeaderTitle_Text.enableWordWrapping = false;
                HeaderTitle_Text.overflowMode = TextOverflowModes.Truncate;
            }

            // Body
            {
                Body = UnityObject.Create("Body")
                      .SetParent(Container_Rect)
                      .AddComponent(out RectTransform Body_Rect)
                      .AddComponent(out VerticalLayoutGroup Body_Layout);

                Body_Layout.childControlHeight = true;
                Body_Layout.childControlWidth = true;
                Body_Layout.childForceExpandHeight = false;
                Body_Layout.childForceExpandWidth = false;

                BodyArea = UnityObject.Create("Display area")
                    .SetParent(Body_Rect)
                    .AddComponent(out RectTransform BodyArea_Rect)
                    .AddComponent(out VerticalLayoutGroup BodyArea_Layout);

                BodyArea_Layout.childControlHeight = true;
                BodyArea_Layout.childControlWidth = true;
                BodyArea_Layout.childForceExpandHeight = false;
                BodyArea_Layout.childForceExpandWidth = false;
                BodyArea_Layout.padding = new(2, 2, 2, 2);

                BodyBack = UnityObject.Create("Background")
                    .SetParent(BodyArea_Rect)
                    .AddComponent(out RectTransform BodyBack_Rect)
                    .AddComponent(out LayoutElement BodyBack_Element)
                    .AddComponent(out Outline BodyBack_Outline)
                    .AddComponent(out Image BodyBack_Image);

                BodyBack_Element.ignoreLayout = true;

                BodyBack_Rect.anchorMin = new(0, 0);
                BodyBack_Rect.anchorMax = new(1, 1);
                BodyBack_Rect.offsetMin = new(0, 0);
                BodyBack_Rect.offsetMax = new(0, 0);

                BodyBack_Image.color = COLOR_BACKGROUND;
                BodyBack_Outline.effectColor = COLOR_BACKGROUND_OUTLINE;

                BodyTooltip = UnityObject.Create("Text")
                    .SetParent(BodyArea_Rect)
                    .AddComponent(out BodyTooltip_Text)
                    .AddComponent(out BodyTooltip_Element);

                BodyTooltip_Text.font = DisplayUtils.GAME_FONT;
                BodyTooltip_Text.fontSize = TOOLTIP_SIZE;
                BodyTooltip_Text.color = COLOR_TEXT;
                BodyTooltip_Text.enableWordWrapping = true;
                BodyTooltip_Text.overflowMode = TextOverflowModes.Truncate;

                BodyTooltip_Element.layoutPriority = 2;
                BodyTooltip_Element.preferredHeight = 80f;

                BodyTooltip_Element.enabled = false;
            }

            // Footer
            {
                Footer = UnityObject.Create("Footer")
                    .SetParent(Container_Rect)
                    .AddComponent(out RectTransform Footer_Rect)
                    .AddComponent(out ContentSizeFitter Footer_Fitter)
                    .AddComponent(out HorizontalLayoutGroup Footer_Layout);

                Footer_Layout.childControlHeight = true;
                Footer_Layout.childControlWidth = true;
                Footer_Layout.childForceExpandHeight = false;
                Footer_Layout.childForceExpandWidth = false;

                Footer_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                FooterArea = UnityObject.Create("Display area")
                    .SetParent(Footer_Rect)
                    .AddComponent(out RectTransform FooterArea_Rect)
                    .AddComponent(out HorizontalLayoutGroup FooterArea_Layout);

                FooterArea_Layout.childControlHeight = true;
                FooterArea_Layout.childControlWidth = true;
                FooterArea_Layout.childForceExpandHeight = false;
                FooterArea_Layout.childForceExpandWidth = false;
                FooterArea_Layout.spacing = 4;

                Tag_Type = CreateTag("Type", Color.yellow, FooterArea_Rect, out Tag_Type_Text);
                Tag_Synchronized = CreateTag("Client synchronize", Color.cyan, FooterArea_Rect, out _);
                Tag_NeedsRestart = CreateTag("Needs restart", new Color32(245, 96, 66, 255), FooterArea_Rect, out _);
                Tag_DefaultValue = CreateTag("Default value", new Color32(191, 191, 191, 255), FooterArea_Rect, out Tag_DefaultValue_Text);
                Tag_Experimental = CreateTag("Experimental", new Color32(181, 2, 2, 255), FooterArea_Rect, out _);
            }

            DisplayConfig(null);
        }

        public virtual void DisplayConfig(Configuration Config)
        {
            if (Config == null)
            {
                Container.SetActive(false);
            }
            else
            {
                Container.SetActive(true);
                HeaderTitle_Text.SetText(Config.Name);
                BodyArea.SetActive(Config.HasTooltip);
                BodyTooltip_Text.SetText(Config.Tooltip);
                BodyTooltip_Text.ForceMeshUpdate(true, true);

                Tag_NeedsRestart.SetActive(Config.NeedsRestart);
                Tag_Synchronized.SetActive(Config.Synchronized);
                Tag_Experimental.SetActive(Config.Experimental);
                Tag_Type_Text.SetText($"Type: {Config.Type.Name}");
                Tag_DefaultValue_Text.SetText($"Default value: {Config.Default}");
                BodyTooltip_Element.enabled = BodyTooltip_Text.preferredHeight > 80f;
            }
        }

        protected virtual GameObject CreateTag(string text, Color textColor, RectTransform parent, out TextMeshProUGUI Text)
        {
            GameObject tag = UnityObject.Create("Tag " + text)
                .SetParent(parent)
                .AddComponent(out RectTransform Tag_Rect)
                .AddComponent(out HorizontalLayoutGroup Tag_Layout);

            Tag_Layout.childControlHeight = true;
            Tag_Layout.childControlWidth = true;
            Tag_Layout.childForceExpandHeight = false;
            Tag_Layout.childForceExpandWidth = false;

            UnityObject.Create("Background")
                .SetParent(Tag_Rect)
                .AddComponent(out LayoutElement Back_Element)
                .AddComponent(out RectTransform Back_Rect)
                .AddComponent(out Image Back_Image)
                .AddComponent(out Outline Back_Outline);

            Back_Element.ignoreLayout = true;

            Back_Rect.anchorMin = new(0, 0);
            Back_Rect.anchorMax = new(1, 1);
            Back_Rect.offsetMin = new(0, 0);
            Back_Rect.offsetMax = new(0, 0);

            Back_Image.color = COLOR_BACKGROUND;
            Back_Outline.effectColor = COLOR_BACKGROUND_OUTLINE;

            UnityObject.Create("Text")
                .SetParent(Tag_Rect)
                .AddComponent(out TextMeshProUGUI Title_Text);

            Title_Text.font = DisplayUtils.GAME_FONT;
            Title_Text.fontSize = TAG_SIZE;
            Title_Text.margin = new(2, 0, 2, 0);
            Title_Text.color = textColor;
            Title_Text.enableWordWrapping = false;
            Title_Text.overflowMode = TextOverflowModes.Truncate;

            Title_Text.SetText(text);

            Text = Title_Text;
            return tag;
        }
    }
}
