using Amrv.ConfigurableCompany.content.display.component;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class ConfigurationMenu
    {
        public static Color COLOR_VIEWPORT = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;
        public static Color COLOR_HANDLE => DisplayUtils.COLOR_SCROLLBAR_HANDLE;
        public static Color COLOR_HANDLE_BACKGROUND => DisplayUtils.COLOR_SCROLLBAR_BACKGROUND;
        public const int SCROLLBAR_WIDTH = 12;
        public const int SCROLLBAR_SPACING = 8;

        protected readonly Dictionary<string, ConfigurationCategoryDisplay> Categories = new();

        public readonly ConfigurationScreen Screen;
        protected readonly GameObject ScrollView;
        protected readonly GameObject Viewport;
        protected readonly GameObject Container;
        protected readonly GameObject Scrollbar;
        protected readonly GameObject SlidingArea;
        protected readonly GameObject SlidingBar;
        protected readonly GameObject SlidingLine;

        public GameObject CategoriesContainer => Container;

        protected internal ConfigurationMenu(GameObject parent, ConfigurationScreen owner)
        {
            Screen = owner;
            ScrollView = UnityObject.Create("Scroll view")
                .SetParent(parent)
                .AddComponent(out RectTransform ScrollView_Rect)
                .AddComponent(out NoDragScrollRect ScrollView_Scroll);

            ScrollView_Rect.anchorMin = new(0, 0);
            ScrollView_Rect.anchorMax = new(1, 1);
            ScrollView_Rect.offsetMin = new(0, 0);
            ScrollView_Rect.offsetMax = new(0, 0);

            Viewport = UnityObject.Create("Viewport")
                .SetParent(ScrollView_Rect)
                .AddComponent(out RectTransform Viewport_Rect)
                .AddComponent(out Image Viewport_Image)
                .AddComponent(out Outline Viewport_Outline)
                .AddComponent(out RectMask2D _);

            Viewport_Rect.pivot = new(0, 1);
            Viewport_Rect.anchorMin = new(0, 0);
            Viewport_Rect.anchorMax = new(1, 1);
            Viewport_Rect.offsetMax = new(-(SCROLLBAR_WIDTH + SCROLLBAR_SPACING), 0);
            Viewport_Rect.offsetMin = new(0, 0);

            Viewport_Image.color = COLOR_VIEWPORT;

            Viewport_Outline.effectColor = DisplayUtils.COLOR_VIEWPORT_OUTLINE;

            Container = UnityObject.Create("Container")
                .SetParent(Viewport_Rect)
                .AddComponent(out RectTransform Container_Rect)
                .AddComponent(out VerticalLayoutGroup Container_Layout)
                .AddComponent(out ContentSizeFitter Container_Fitter);

            Container_Rect.anchorMin = new(0, 1);
            Container_Rect.anchorMax = new(1, 1);
            Container_Rect.offsetMin = new(0, 0);
            Container_Rect.offsetMax = new(0, 0);
            Container_Rect.pivot = new(0, 1);

            Container_Layout.childControlHeight = true;
            Container_Layout.childControlWidth = true;
            Container_Layout.childForceExpandHeight = false;
            Container_Layout.childForceExpandWidth = false;

            Container_Layout.spacing = 3;

            Container_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Scrollbar = UnityObject.Create("Scroller")
                .SetParent(ScrollView_Rect)
                .AddComponent(out RectTransform Scrollbar_Rect)
                .AddComponent(out Scrollbar Scrollbar_Scroll);

            Scrollbar_Rect.anchorMin = new(1, 0);
            Scrollbar_Rect.anchorMax = new(1, 1);
            Scrollbar_Rect.offsetMin = new(0, 0);
            Scrollbar_Rect.offsetMax = new(0, 0);
            Scrollbar_Rect.pivot = new(1, 1);

            Scrollbar_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SCROLLBAR_WIDTH);

            SlidingArea = UnityObject.Create("Sliding area")
                .SetParent(Scrollbar_Rect)
                .AddComponent(out RectTransform SlidingArea_Rect);

            SlidingArea_Rect.anchorMin = new(0, 0);
            SlidingArea_Rect.anchorMax = new(1, 1);
            SlidingArea_Rect.offsetMin = new(0, 0);
            SlidingArea_Rect.offsetMax = new(0, 0);

            SlidingLine = UnityObject.Create("Sliding line")
                .SetParent(SlidingArea_Rect)
                .AddComponent(out Outline SlidingLine_Outline)
                .AddComponent(out RectTransform SliderLine_Rect)
                .AddComponent(out Image SliderLine_Image);

            SliderLine_Rect.anchorMin = new(.3f, 0);
            SliderLine_Rect.anchorMax = new(.7f, 1);
            SliderLine_Rect.offsetMin = new(0, 0);
            SliderLine_Rect.offsetMax = new(0, 0);

            SliderLine_Image.color = COLOR_HANDLE_BACKGROUND;
            SlidingLine_Outline.effectColor = DisplayUtils.COLOR_SCROLLBAR_HANDLE_OUTLINE;

            SlidingBar = UnityObject.Create("Sliding bar")
                .SetParent(SlidingArea_Rect)
                .AddComponent(out RectTransform SliderBar_Rect)
                .AddComponent(out Outline SliderBar_Outline)
                .AddComponent(out Image SlidingBar_Image);

            SliderBar_Rect.offsetMin = new(0, 0);
            SliderBar_Rect.offsetMax = new(0, 0);

            SlidingBar_Image.color = COLOR_HANDLE;

            SliderBar_Outline.effectColor = DisplayUtils.COLOR_SCROLLBAR_HANDLE_OUTLINE;

            Scrollbar_Scroll.targetGraphic = SlidingBar_Image;
            Scrollbar_Scroll.handleRect = SliderBar_Rect;
            Scrollbar_Scroll.direction = UnityEngine.UI.Scrollbar.Direction.BottomToTop;
            Scrollbar_Scroll.navigation = DisplayUtils.NO_NAVIGATION;
            Scrollbar_Scroll.colors = DisplayUtils.COLOR_TINT_DEFAULT;

            ScrollView_Scroll.scrollSensitivity = 3;
            ScrollView_Scroll.content = Container_Rect;
            ScrollView_Scroll.horizontal = false;
            ScrollView_Scroll.viewport = Viewport_Rect;
            ScrollView_Scroll.verticalScrollbar = Scrollbar_Scroll;
            ScrollView_Scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        }

        public void Add(ConfigurationCategoryDisplay category)
        {
            string ID = category.Category.ID;

            if (TryGet(ID, out ConfigurationCategoryDisplay _))
            {
                Remove(ID);
            }

            Categories[ID] = category;
            category.AddToParent(this);
        }

        public void Remove(ConfigurationCategoryDisplay category) => Remove(category.Category.ID);
        public void Remove(string categoryID)
        {
            if (TryGet(categoryID, out ConfigurationCategoryDisplay category))
            {
                category.RemoveFromParent(this);
                Categories.Remove(categoryID);
            }
        }

        public bool TryGet(string id, out ConfigurationCategoryDisplay result) => Categories.TryGetValue(id, out result);

        protected internal virtual void RefreshContent()
        {
            Dictionary<string, ConfigurationCategoryDisplay> remainingToRemove = new(Categories);

#if DEBUG
            Console.WriteLine($"ConfigurationMenu::RefreshContent");
#endif
            foreach (ConfigurationCategory category in ConfigurationCategory.Categories)
            {
                if (TryGet(category.ID, out ConfigurationCategoryDisplay result))
                {
#if DEBUG
                    Console.WriteLine($"ConfigurationMenu::RefreshContent [MODIFY]  {category.ID}");
#endif
                    result.RefreshCategory();
                    remainingToRemove.Remove(category.ID);
                }
                else
                {
#if DEBUG
                    Console.WriteLine($"ConfigurationMenu::RefreshContent [ADDED]   {category.ID}");
#endif
                    ConfigurationCategoryDisplay display = new(category);
                    Add(display);
                    display.RefreshContent();
                }
            }

            foreach (KeyValuePair<string, ConfigurationCategoryDisplay> invalids in remainingToRemove)
            {
#if DEBUG
                Console.WriteLine($"ConfigurationMenu::RefreshContent [REMOVED] {invalids.Key}");
#endif
                Remove(invalids.Key);
                invalids.Value.Delete();
            }
        }

        protected internal virtual void LoadConfigs()
        {
            foreach (ConfigurationCategoryDisplay display in Categories.Values)
            {
                display.LoadConfigs();
            }
        }

        protected internal virtual void SaveConfigs()
        {
            foreach (ConfigurationCategoryDisplay display in Categories.Values)
            {
                display.SaveConfigs();
            }
        }
    }
}
