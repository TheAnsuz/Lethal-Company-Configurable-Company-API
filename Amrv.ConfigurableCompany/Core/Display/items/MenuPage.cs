using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuPage
    {
        public static MenuPage CreatePage(Transform parent, CPage page)
        {
            return new(Object.Instantiate(MenuPrefabs.Page, parent, false), page);
        }

        private CPage Page { get; set; }
        private GameObject PageObject { get; set; }
        private TextMeshProUGUI Title { get; set; }
        private TextMeshProUGUI Description { get; set; }

        private MenuPage(GameObject pageObject, CPage page)
        {
            PageObject = pageObject;
            Page = page;

            //PageObject.AddComponent<NoDrawGraphic>();
            PageObject.GetComponent<Button>().onClick.AddListener(OnClick);

            Title = pageObject.FindChild("Name").GetComponent<TextMeshProUGUI>();
            Description = pageObject.FindChild("Info").GetComponent<TextMeshProUGUI>();

            Title.SetText(Page.Name);
            Description.SetText(Page.Description);
        }

        public void SetTitle(string title)
        {
            Title.text = title;
        }

        public void SetDescription(string description)
        {
            Description.text = description;
        }

        private void OnClick(/*object sender, PointerEventData e*/)
        {
            MenuEventRouter.OnClick_ShowPage(Page);
        }

        internal void Destroy()
        {
            Object.Destroy(PageObject);
            PageObject = null;
            Page = null;
            Title = null;
            Description = null;
        }

#if DEBUG
        ~MenuPage()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuPage deleted");
        }
#endif
    }
}
