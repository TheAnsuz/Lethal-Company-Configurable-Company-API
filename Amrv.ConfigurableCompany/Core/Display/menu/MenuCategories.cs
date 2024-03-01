using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuCategories : IMenuPart
    {
        private readonly MenuBind Bind;
        private readonly GameObject Panel;

        private readonly GameObject Content;

        protected readonly Dictionary<CCategory, MenuCategory> Categories = [];

        internal MenuCategories(MenuBind bind)
        {
            Bind = bind;
            Panel = Bind.Menu.FindChild("Panel");
            Content = Panel.FindChild("Scroll View/Viewport/Content");
        }

        public void AddCategory(CCategory category)
        {
            MenuCategory item = MenuCategory.CreateCategory(Content.transform, category);
            Categories.Add(category, item);
            item.SetVisible(category.Page.Equals(Bind.Pages.CurrentPage));
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

        public void Destroy()
        {

        }

        public void UpdateContent()
        {
            foreach (CCategory page in CCategory.Storage.Values)
            {
                AddCategory(page);
            }
        }

        public void UpdateSelf()
        {

        }
    }
}
