using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuPages : IMenuPart
    {
        protected Reference<MenuBind> Bind;

        protected GameObject PageContainer;
        protected GameObject PageName;
        protected TextMeshProUGUI PageName_Text;

        protected readonly Dictionary<CPage, MenuPage> Pages = new(CPage.Storage.Count);

        private CPage _currentPage;
        public CPage CurrentPage
        {
            get => _currentPage; set
            {
                if (_currentPage == value)
                    return;

                PageName.SetActive(!string.IsNullOrEmpty(value?.Name ?? null));
                PageName_Text.SetText(value?.Name ?? null);
                Bind.Item.Categories.Item.DisplayPage(value);
                _currentPage = value;
            }
        }

        internal MenuPages(Reference<MenuBind> bind)
        {
            Bind = bind;

            PageContainer = Bind.Item.Menu.FindChild("Pages/Scroll View/Viewport/Content");
            PageName = Bind.Item.Menu.FindChild("Info/Page name");
            PageName_Text = PageName.FindChild("Area/Text").GetComponent<TextMeshProUGUI>();
        }

        public void AddPage(CPage page) => Pages[page] = MenuPage.CreatePage(PageContainer.transform, page);

        public MenuPage GetPage(CPage page) => Pages[page];

        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuPages deletion in progress ({Pages.Count} pages)");

            Bind = null;

            foreach (MenuPage page in Pages.Values)
            {
                page.Destroy();
            }
            Pages.Clear();

            Object.Destroy(PageContainer);
            Object.Destroy(PageName);

            PageContainer = null;
            PageName = null;
            PageName_Text = null;
        }

        public IEnumerator UpdateContent()
        {
            foreach (CPage page in CPage.Storage.Values)
            {
                AddPage(page);
                CurrentPage ??= page;
            }
            yield break;
        }

        public IEnumerator UpdateSelf()
        {
            yield break;
        }

#if DEBUG
        ~MenuPages()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuPages deleted");
        }
#endif
    }
}
