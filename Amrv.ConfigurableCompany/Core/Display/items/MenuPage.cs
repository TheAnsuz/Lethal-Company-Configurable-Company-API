using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuPage
    {
        public static MenuPage CreatePage(Transform parent, CPage page)
        {
            return new(Object.Instantiate(MenuPresets.Page, parent, false), page);
        }

        private readonly CPage Page;
        private readonly GameObject PageObject;
        private readonly TextMeshProUGUI Title;
        private readonly TextMeshProUGUI Description;

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

        }
    }
}
