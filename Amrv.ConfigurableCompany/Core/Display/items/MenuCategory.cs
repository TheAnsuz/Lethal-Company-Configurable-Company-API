using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Core.IO;
using System;
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
            return new(UnityEngine.Object.Instantiate(MenuPresets.Category, parent, false), category);
        }

        private readonly GameObject Container;
        public readonly CCategory Category;
        public readonly GameObject Content;
        private readonly GameObject NameArea;
        private readonly GameObject Shadow;

        public readonly Image Sidebar_Image;
        public readonly TextMeshProUGUI Name_Text;
        public readonly Image Name_Background;

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

        private void OnClickHeader(object sender, PointerEventData e)
        {
            bool active = Content.transform.childCount != 0 && !Content.activeSelf;
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
    }
}
