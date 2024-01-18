#if DEBUG
using System;
#endif
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class ConfigurationCategoryDisplay
    {
        public const int SIDEBAR_WIDTH = 3;
        public const int SIDEBAR_SPACING = 3;
        public static float FONT_SIZE_MIN => 8;
        public static float FONT_SIZE_MAX => 16;
        public static readonly Color HEADER_MASK_SHADOW_COLOR = new(0, 0, 0, .8f);

        public ConfigurationMenu Menu { get; protected set; }
        public readonly ConfigurationCategory Category;
        protected internal readonly GameObject Container;

        public bool Visible
        {
            get => ContentArea.activeSelf;
            set
            {
                HeaderMaskShadowExtra.SetActive(!value);
                ContentArea.SetActive(value);
            }
        }
        public bool IsEmpty => Configurations.Count == 0;
        protected readonly Dictionary<string, ConfigurationItemDisplay> Configurations = new();

        protected readonly GameObject HeaderArea;
        protected readonly GameObject HeaderMask;
        protected readonly GameObject HeaderMaskShadow;
        protected readonly GameObject HeaderMaskShadowExtra;
        protected readonly GameObject Title;
        protected readonly TextMeshProUGUI Title_Text;
        protected readonly GameObject Sidebar;
        protected readonly Image Sidebar_Image;
        protected readonly GameObject ContentArea;

        protected internal ConfigurationCategoryDisplay(ConfigurationCategory category)
        {
            Category = category;

            Container = UnityObject.Create("Category " + category.ID)
                .AddComponent(out RectTransform Container_Rect)
                .AddComponent(out VerticalLayoutGroup Container_Layout)
                .AddComponent(out ContentSizeFitter Container_Fitter);

            Container_Rect.anchorMin = new(0, 1);
            Container_Rect.anchorMax = new(0, 1);
            Container_Rect.offsetMin = new(0, 0);
            Container_Rect.offsetMax = new(0, 0);

            Container_Layout.childControlHeight = true;
            Container_Layout.childControlWidth = true;
            Container_Layout.childForceExpandWidth = true;
            Container_Layout.childForceExpandHeight = false;

            Container_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            HeaderArea = UnityObject.Create("Header")
                .SetParent(Container_Rect)
                .AddComponent(out RectTransform HeaderArea_Rect)
                //.AddComponent(out HeaderArea_Image)
                .AddComponent(out LayoutElement _)
                .AddComponent(out Button HeaderArea_Button)
                .AddComponent(out HorizontalLayoutGroup HeaderArea_Layout);

            HeaderArea_Layout.padding.left = SIDEBAR_WIDTH + SIDEBAR_SPACING;
            HeaderArea_Layout.childControlWidth = true;
            HeaderArea_Layout.childControlHeight = true;
            HeaderArea_Layout.childForceExpandHeight = false;
            HeaderArea_Layout.childForceExpandWidth = false;

            HeaderArea_Button.colors = DisplayUtils.COLOR_TINT_DEFAULT;
            HeaderArea_Button.navigation = DisplayUtils.NO_NAVIGATION;
            HeaderArea_Button.onClick.AddListener(OnClick);

            HeaderMask = UnityObject.Create("Header mask")
                .SetParent(HeaderArea_Rect)
                .AddComponent(out LayoutElement HeaderMask_Element)
                .AddComponent(out RectTransform HeaderMask_Rect)
                .AddComponent(out Image HeaderMask_Image);

            HeaderMask_Element.ignoreLayout = true;

            HeaderMask_Rect.anchorMin = new(0, 0);
            HeaderMask_Rect.anchorMax = new(1, 1);
            HeaderMask_Rect.offsetMin = new(0, 0);
            HeaderMask_Rect.offsetMax = new(0, 0);

            HeaderMask_Image.color = category.Color;

            HeaderMaskShadow = UnityObject.Create("Header mask shadow")
                .SetParent(HeaderArea_Rect)
                .AddComponent(out LayoutElement HeaderMaskShadow_Element)
                .AddComponent(out RectTransform HeaderMaskShadow_Rect)
                .AddComponent(out Image HeaderMaskShadow_Image);

            HeaderMaskShadow_Element.ignoreLayout = true;

            HeaderMaskShadow_Rect.anchorMin = new(0, 0);
            HeaderMaskShadow_Rect.anchorMax = new(1, 1);
            HeaderMaskShadow_Rect.offsetMin = new(0, 0);
            HeaderMaskShadow_Rect.offsetMax = new(0, 0);

            HeaderMaskShadow_Image.color = HEADER_MASK_SHADOW_COLOR;

            HeaderMaskShadowExtra = UnityObject.Create("Header mask shadow")
                .SetParent(HeaderArea_Rect)
                .AddComponent(out LayoutElement HeaderMaskShadowExtra_Element)
                .AddComponent(out RectTransform HeaderMaskShadowExtra_Rect)
                .AddComponent(out Image HeaderMaskShadowExtra_Image);

            HeaderMaskShadowExtra_Element.ignoreLayout = true;

            HeaderMaskShadowExtra_Rect.anchorMin = new(0, 0);
            HeaderMaskShadowExtra_Rect.anchorMax = new(1, 1);
            HeaderMaskShadowExtra_Rect.offsetMin = new(0, 0);
            HeaderMaskShadowExtra_Rect.offsetMax = new(0, 0);

            HeaderMaskShadowExtra_Image.color = HEADER_MASK_SHADOW_COLOR;

            HeaderMaskShadowExtra.SetActive(false);

            //HeaderArea_Image.color = category.ColorDarker;

            Title = UnityObject.Create("Title")
                .SetParent(HeaderArea_Rect)
                .AddComponent(out RectTransform _)
                .AddComponent(out Title_Text);

            Title_Text.font = DisplayUtils.GAME_FONT;
            Title_Text.enableAutoSizing = true;
            Title_Text.autoSizeTextContainer = true;
            Title_Text.enableWordWrapping = false;
            Title_Text.overflowMode = TextOverflowModes.Truncate;
            Title_Text.fontSizeMin = FONT_SIZE_MIN;
            Title_Text.fontSizeMax = FONT_SIZE_MAX;
            Title_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            Title_Text.SetText(category.Name);

            ContentArea = UnityObject.Create("Content")
                .SetParent(Container_Rect)
                .AddComponent(out RectTransform _)
                .AddComponent(out LayoutElement _)
                //.AddComponent(out Image ContentArea_Image)
                .AddComponent(out VerticalLayoutGroup ContentArea_Layout);

            ContentArea_Layout.childControlWidth = true;
            ContentArea_Layout.childControlHeight = true;
            ContentArea_Layout.childForceExpandHeight = true;
            ContentArea_Layout.childForceExpandWidth = true;
            ContentArea_Layout.padding.left = SIDEBAR_WIDTH + SIDEBAR_SPACING;

            //ContentArea_Image.color = category.ColorDarkest;

            Sidebar = UnityObject.Create("Sidebar")
                .SetParent(Container_Rect)
                .AddComponent(out RectTransform Sidebar_Rect)
                .AddComponent(out LayoutElement Sidebar_Element)
                .AddComponent(out Sidebar_Image);

            Sidebar_Element.ignoreLayout = true;
            Sidebar_Element.layoutPriority = 10;

            Sidebar_Rect.anchorMin = new(0, 0);
            Sidebar_Rect.anchorMax = new(0, 1);
            Sidebar_Rect.pivot = new(0, 1);
            Sidebar_Rect.offsetMin = new(0, 0);
            Sidebar_Rect.offsetMax = new(0, 0);

            Sidebar_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SIDEBAR_WIDTH);

            Sidebar_Image.color = category.Color;
            Visible = !IsEmpty;

            if (IsEmpty && Category.HideIfEmpty)
                Container.SetActive(false);
            else
                Container.SetActive(true);
        }

        protected virtual void OnClick()
        {
            if (Visible)
            {
                Visible = false;
            }
            else if (!IsEmpty)
            {
                Visible = true;
            }
        }

        protected internal virtual void RefreshCategory()
        {
            Title_Text.text = Category.Name;
            Sidebar_Image.color = Category.Color;
        }

        protected internal virtual void AddToParent(ConfigurationPageDisplay configurationPage)
        {
            Menu = configurationPage.Menu;
            Container.transform.SetParent(configurationPage.Container.transform, false);
        }

        protected internal virtual void RemoveFromParent(ConfigurationPageDisplay configurationPage)
        {
            Menu = null;
            Container.transform.parent = null;
        }

        protected internal virtual void Delete()
        {
            UnityEngine.Object.Destroy(Container);
        }

        public void Add(ConfigurationItemDisplay item)
        {
            string ID = item.Config.ID;

            if (TryGet(ID, out ConfigurationItemDisplay _))
            {
                Remove(ID);
            }

            Configurations[ID] = item;
            item.AddToParent(this, ContentArea);
        }

        public void Remove(ConfigurationItemDisplay item) => Remove(item.Config.ID);
        public void Remove(string configID)
        {
            if (TryGet(configID, out ConfigurationItemDisplay display))
            {
                display.RemoveFromParent(this, ContentArea);
                Configurations.Remove(configID);
            }
        }

        public bool TryGet(string id, out ConfigurationItemDisplay result) => Configurations.TryGetValue(id, out result);

        protected internal virtual void RefreshContent()
        {
            bool wasEmpty = IsEmpty;

            Dictionary<string, ConfigurationItemDisplay> remainingToRemove = new(Configurations);

#if DEBUG
            Console.WriteLine($"Refreshing configs of category {Category.ID}");
#endif
            foreach (Configuration config in Configuration.Configs)
            {
                if (config.Category == Category)
                {
                    if (TryGet(config.ID, out ConfigurationItemDisplay result))
                    {
#if DEBUG
                        Console.WriteLine($"-- [MODIF]: {config.ID}");
#endif
                        result.RefreshDisplay();
                        remainingToRemove.Remove(config.ID);
                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine($"-- [ADDED]: {config.ID}");
#endif
                        Add(config.Type.CreateDisplay(config));
                    }
                }
                else
                {
                    if (TryGet(config.ID, out ConfigurationItemDisplay result))
                    {
                        // WFT is this config doing here
                        remainingToRemove[config.ID] = result;
                    }
                }
            }

            foreach (KeyValuePair<string, ConfigurationItemDisplay> invalids in remainingToRemove)
            {
#if DEBUG
                Console.WriteLine($"-- [REMOV]: {invalids.Key}");
#endif
                Remove(invalids.Key);
                invalids.Value.Delete();
            }

            if (IsEmpty && Category.HideIfEmpty)
                Container.SetActive(false);
            else
                Container.SetActive(true);


            if (IsEmpty)
                Visible = false;
            else if (wasEmpty)
                Visible = true;
        }

        protected internal virtual void SaveConfigs()
        {
            foreach (ConfigurationItemDisplay display in Configurations.Values)
            {
                display.SetToConfig();
            }
        }

        protected internal virtual void LoadConfigs()
        {
            foreach (ConfigurationItemDisplay display in Configurations.Values)
            {
                display.GetFromConfig();
            }
        }
    }
}
