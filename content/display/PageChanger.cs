using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class PageChanger
    {
        public readonly ConfigurationScreen Screen;

        public readonly GameObject Container;
        public readonly GameObject ArrowLeft;
        public readonly GameObject ArrowRight;
        public readonly GameObject Title;
        public readonly TextMeshProUGUI Title_Text;

        internal PageChanger(GameObject pageChangerArea, ConfigurationScreen configurationScreen)
        {
            Screen = configurationScreen;

            Container = UnityObject.Create("Page changer")
                .SetParent(pageChangerArea)
                .AddComponent(out RectTransform Container_Rect)
                .AddComponent(out Image Container_Image)
                .AddComponent(out Outline Container_Outline)
                .AddComponent(out HorizontalLayoutGroup Container_Layout);

            Container_Rect.anchorMin = new(0, 0);
            Container_Rect.anchorMax = new(1, 1);
            Container_Rect.offsetMin = new(0, 0);
            Container_Rect.offsetMax = new(0, 0);

            Container_Image.color = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;

            Container_Outline.effectColor = DisplayUtils.COLOR_VIEWPORT_OUTLINE;

            Container_Layout.childAlignment = TextAnchor.MiddleCenter;
            Container_Layout.childControlHeight = true;
            Container_Layout.childControlWidth = true;
            Container_Layout.childForceExpandHeight = true;
            Container_Layout.childForceExpandWidth = true;

            ArrowLeft = UnityObject.Create("Left")
                .SetParent(Container_Rect)
                .AddComponent(out Button ArrowLeft_Button)
                .AddComponent(out TextMeshProUGUI ArrowLeft_Text);

            ArrowLeft_Text.fontStyle = FontStyles.Bold;
            ArrowLeft_Text.font = DisplayUtils.GAME_FONT;
            ArrowLeft_Text.fontSize = 22;
            ArrowLeft_Text.color = DisplayUtils.COLOR_INPUT_SELECTION;
            ArrowLeft_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            ArrowLeft_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            ArrowLeft_Text.margin = new(4, 0, 4, 0);
            ArrowLeft_Text.SetText("<");

            ArrowLeft_Button.onClick.AddListener(PrevPage);

            Title = UnityObject.Create("Title")
                .SetParent(Container_Rect)
                .AddComponent(out LayoutElement Title_Element)
                .AddComponent(out Title_Text);

            Title_Element.flexibleWidth = 200;
            Title_Element.preferredWidth = 200;

            Title_Text.font = DisplayUtils.GAME_FONT;
            Title_Text.fontSize = 18;
            Title_Text.color = DisplayUtils.COLOR_INPUT_TEXT;
            Title_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            Title_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            Title_Text.enableWordWrapping = false;

            ArrowRight = UnityObject.Create("Right")
                .SetParent(Container_Rect)
                .AddComponent(out Button ArrowRight_Button)
                .AddComponent(out TextMeshProUGUI ArrowRight_Text);

            ArrowRight_Text.fontStyle = FontStyles.Bold;
            ArrowRight_Text.font = DisplayUtils.GAME_FONT;
            ArrowRight_Text.fontSize = 22;
            ArrowRight_Text.color = DisplayUtils.COLOR_INPUT_SELECTION;
            ArrowRight_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            ArrowRight_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            ArrowRight_Text.margin = new(4, 0, 4, 0);
            ArrowRight_Text.SetText(">");

            ArrowRight_Button.onClick.AddListener(NextPage);
            Refresh();
        }

        public void NextPage()
        {
            ConfigurationPageDisplay page = Screen.ConfigurationMenu.NextPage();

            Title_Text.SetText(page.Page.Name);
        }

        public void PrevPage()
        {
            ConfigurationPageDisplay page = Screen.ConfigurationMenu.PrevPage();

            Title_Text.SetText(page.Page.Name);
        }

        public void Refresh()
        {
            Title_Text.SetText(Screen.ConfigurationMenu?.CurrentPage.Page.Name ?? "");
        }
    }
}
