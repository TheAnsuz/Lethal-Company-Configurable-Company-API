using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuPages : IMenuPart
    {
        protected readonly MenuBind Bind;

        protected readonly GameObject PageContainer;
        protected readonly GameObject PageName;
        protected readonly TextMeshProUGUI PageName_Text;

        protected readonly Dictionary<CPage, MenuPage> Pages = [];

        private CPage _currentPage;
        public CPage CurrentPage
        {
            get => _currentPage; set
            {
                if (_currentPage == value)
                    return;

                PageName.SetActive(!string.IsNullOrEmpty(value?.Name ?? null));
                PageName_Text.SetText(value?.Name ?? null);
                Bind.Categories.DisplayPage(value);
                _currentPage = value;
            }
        }

        internal MenuPages(MenuBind bind)
        {
            Bind = bind;

            PageContainer = Bind.Menu.FindChild("Pages/Scroll View/Viewport/Content");
            PageName = Bind.Menu.FindChild("Info/Page name");
            PageName_Text = PageName.FindChild("Area/Text").GetComponent<TextMeshProUGUI>();
        }

        public void AddPage(CPage page) => Pages[page] = MenuPage.CreatePage(PageContainer.transform, page);

        public MenuPage GetPage(CPage page) => Pages[page];

        public void Destroy()
        {
            foreach (MenuPage page in Pages.Values)
            {
                page.Destroy();
            }
            Pages.Clear();
        }

        public void UpdateContent()
        {
            foreach (CPage page in CPage.Storage.Values)
            {
                AddPage(page);
                CurrentPage ??= page;
            }
        }

        public void UpdateSelf()
        {

        }
    }
}
