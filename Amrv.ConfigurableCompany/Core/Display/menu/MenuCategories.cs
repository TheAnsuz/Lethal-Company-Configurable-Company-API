using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuCategories : IMenuPart
    {
        private Reference<MenuBind> Bind;
        private GameObject Panel;

        private GameObject Content;

        protected Dictionary<CCategory, MenuCategory> Categories = new(CCategory.Storage.Count);

        internal MenuCategories(Reference<MenuBind> bind)
        {
            Bind = bind;
            Panel = Bind.Item.Menu.FindChild("Panel");
            Content = Panel.FindChild("Scroll View/Viewport/Content");
        }

        public void AddCategory(CCategory category)
        {
            MenuCategory item = MenuCategory.CreateCategory(Content.transform, category);
            Categories.Add(category, item);
            item.SetVisible(category.Page.Equals(Bind.Item.Pages.Item.CurrentPage));
        }

        public MenuCategory GetCategory(CCategory category)
        {
            return Categories[category];
        }

        internal void DisplayPage(CPage page)
        {
            foreach (var entry in Categories)
            {
                entry.Value.SetVisible(entry.Key.Page.Equals(page));
            }
        }

        [Obsolete("Does nothing on this class")]
        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuCategories deletion in progress ({Categories.Count} categories)");
            foreach (var entry in Categories)
            {
                entry.Value.Destroy();
            }

            UnityEngine.Object.Destroy(Panel);
            UnityEngine.Object.Destroy(Content);

            Bind = null;
            Panel = null;
            Content = null;
            Categories.Clear();
            Categories = null;
        }

        public IEnumerator UpdateContent()
        {
            foreach (CCategory page in CCategory.Storage.Values)
            {
                AddCategory(page);
            }

            yield break;
        }

        [Obsolete("Does nothing on this class")]
        public IEnumerator UpdateSelf()
        {
            yield break;
        }

#if DEBUG
        ~MenuCategories()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuCategories deleted");
        }
#endif
    }
}
