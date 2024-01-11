#if DEBUG
using System;
#endif
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class ConfigurationPageDisplay
    {
        protected readonly Dictionary<string, ConfigurationCategoryDisplay> Categories = new();

        public ConfigurationMenu Menu { get; private set; }
        public int Index => Page.Number;
        public readonly ConfigurationPage Page;
        public readonly GameObject Container;

        protected internal ConfigurationPageDisplay(ConfigurationPage page, ConfigurationMenu Menu)
        {
            Page = page;
            this.Menu = Menu;
            Container = UnityObject.Create($"Page {page.Name}")
                .SetParent(Menu.PagesContainer)
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

            SetVisible(false);
        }

        public void SetVisible(bool visible)
        {
            Container.SetActive(visible);
        }

        public void Delete()
        {
            UnityEngine.Object.Destroy(Container);
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

        protected internal virtual void RefreshCategories()
        {
            Dictionary<string, ConfigurationCategoryDisplay> remainingToRemove = new(Categories);

#if DEBUG
            Console.WriteLine($"ConfigurationPageDisplay::RefreshContent");
#endif
            foreach (ConfigurationCategory category in ConfigurationCategory.Categories)
            {
                if (TryGet(category.ID, out ConfigurationCategoryDisplay result))
                {
#if DEBUG
                    Console.WriteLine($"ConfigurationPageDisplay::RefreshContent [MODIFY]  {category.ID}");
#endif
                    result.RefreshCategory();
                    remainingToRemove.Remove(category.ID);
                }
                else if (category.Page == Page)
                {
#if DEBUG
                    Console.WriteLine($"ConfigurationPageDisplay::RefreshContent [ADDED]   {category.ID}");
#endif
                    ConfigurationCategoryDisplay display = new(category);
                    Add(display);
                    display.RefreshContent();
                }
            }

            foreach (KeyValuePair<string, ConfigurationCategoryDisplay> invalids in remainingToRemove)
            {
#if DEBUG
                Console.WriteLine($"ConfigurationPageDisplay::RefreshContent [REMOVED] {invalids.Key}");
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
