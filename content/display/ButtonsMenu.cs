#if DEBUG
using System;
#endif
using Amrv.ConfigurableCompany.content.display.customObject;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class ButtonsMenu
    {
        protected readonly GameObject Container;
        public readonly ConfigurationScreen Owner;

        protected readonly MenuButton Button_Reset;
        protected readonly MenuButton Button_Information;

        protected internal ButtonsMenu(GameObject parent, ConfigurationScreen owner)
        {
            Owner = owner;

            Container = UnityObject.Create("Buttons")
                .SetParent(parent)
                .AddComponent(out RectTransform Container_Rect)
                .AddComponent(out VerticalLayoutGroup Container_Layout);

            Container_Rect.anchorMin = Vector2.zero;
            Container_Rect.anchorMax = Vector2.one;
            Container_Rect.offsetMin = Vector2.zero;
            Container_Rect.offsetMax = Vector2.zero;

            Container_Layout.padding = new(2, 2, 2, 2);
            Container_Layout.spacing = 6;
            Container_Layout.childControlHeight = true;
            Container_Layout.childControlWidth = true;
            Container_Layout.childForceExpandHeight = false;
            Container_Layout.childForceExpandWidth = false;
            Container_Layout.childAlignment = TextAnchor.LowerLeft;

            Button_Reset = CreateButton("Reset button", "Reset configuration");
            Button_Reset.OnClick.AddListener(OnReset);
            Button_Information = CreateButton("Information", "Information / Guide");
            Button_Information.OnClick.AddListener(OnInformation);
        }

        protected virtual void OnReset()
        {
            IngameMenu.ResetCurrentConfig();
        }

        protected virtual void OnInformation()
        {
            Application.OpenURL("https://i.imgur.com/457DmHb.png");
        }

        protected virtual MenuButton CreateButton(string name, string text)
        {
            return new(name, Container)
            {
                FontSize = 9,
                TextColor = Color.white,
                BackgroundColor = DisplayUtils.COLOR_VIEWPORT_BACKGROUND,
                OutlineColor = DisplayUtils.COLOR_VIEWPORT_OUTLINE,
                Text = text
            };
        }
    }
}
