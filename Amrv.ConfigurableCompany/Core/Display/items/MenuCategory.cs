using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Plugin;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuCategory
    {
        public static MenuCategory CreateCategory(Transform parent, CCategory category)
        {
            return new(UnityEngine.Object.Instantiate(MenuPrefabs.Category, parent, false), category);
        }

        private GameObject Container { get; set; }
        public CCategory Category { get; private set; }
        public GameObject Content { get; private set; }
        private GameObject NameArea { get; set; }
        private GameObject Shadow { get; set; }

        public Image Sidebar_Image { get; private set; }
        public TextMeshProUGUI Name_Text { get; private set; }
        public Image Name_Background { get; private set; }

        private MenuCategory(GameObject container, CCategory category)
        {
            container.name = $"Category {category.ID}";
            Container = container;
            Category = category;
            Content = Container.FindChild("Content area");
            Shadow = Container.FindChild("Name area/Shadow");
            NameArea = Container.FindChild("Name area");
            NameArea.AddComponent<RegionButton>().OnMouseClick += OnClickHeader;

            Sidebar_Image = Container.FindChild("Sidebar").GetComponent<Image>();
            Container.FindChild("Sidebar").GetComponent<Button>().onClick.AddListener(() => SetOpen(!IsOpen()));
            Name_Text = Container.FindChild("Name area/Name").GetComponent<TextMeshProUGUI>();
            Name_Background = Container.FindChild("Name area/Background").GetComponent<Image>();

            SetOpen(IOController.GetCategoryOpenState(category));
            SetColor(category.Color);
            SetName(category.Name);
        }

        private void SetOpen(bool open)
        {
            Content.SetActive(open);
            Shadow.SetActive(!open);
            Container.SetActive(!(Content.transform.childCount == 0 && Category.HideIfEmpty));
        }

        public bool IsOpen() => Content.activeSelf;

        private void OnClickHeader(object sender, PointerEventData e)
        {
            bool active = Content.transform.childCount != 0 && !IsOpen();
            MenuEventRouter.OnAction_ToggleCategory(Category, active);
            SetOpen(active);
        }

        public void SetColor(Color color)
        {
            Sidebar_Image.color = color;
            Name_Background.color = color;
        }

        public void SetName(string name)
        {
            Name_Text.SetText(name);
        }

        public void SetVisible(bool visible)
        {
            Container.SetActive(visible);
        }

        internal void Destroy()
        {
            UnityEngine.Object.Destroy(Container);
            UnityEngine.Object.Destroy(Content);
            UnityEngine.Object.Destroy(NameArea);
            UnityEngine.Object.Destroy(Shadow);

            Container = null;
            Category = null;
            Content = null;
            NameArea = null;
            Shadow = null;

            Sidebar_Image = null;
            Name_Background = null;
            Name_Text = null;
        }

#if DEBUG
        ~MenuCategory()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuCategory deleted");
        }
#endif
    }
}
