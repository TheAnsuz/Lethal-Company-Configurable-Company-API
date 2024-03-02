using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display
{
    internal class MenuBind
    {
        // The game object that contains all the menu
        protected readonly GameObject Container;

        public readonly GameObject Overlay;
        public readonly GameObject ShowMenu;
        public readonly GameObject Menu;

        protected readonly GameObject FileName;
        protected readonly TextMeshProUGUI FileText;

        public readonly MenuToggle Toggler;
        public readonly MenuPages Pages;
        public readonly MenuButtons Buttons;
        public readonly MenuTooltip Tooltip;
        public readonly MenuCategories Categories;
        public readonly MenuSections Sections;
        public readonly MenuConfigs Configs;

        public string Filename
        {
            get => FileText.text;
            set
            {
                FileName.SetActive(!string.IsNullOrEmpty(value));
                FileText.SetText(value);
            }
        }

        private static MenuBind _instance;
        public static MenuBind Create(Transform parent)
        {
            if (_instance != null)
                throw new BuildingException("Can't create MenuBind while there is an existing one");

            _instance = new(parent);

            // Setters
            _instance.Toggler.Open = false;
            _instance.Toggler.Visible = false;
            _instance.Filename = GameNetworkManager.Instance.currentSaveFileName;

            return _instance;
        }

        private MenuBind(Transform parent)
        {
            MenuEventRouter.OnAction_PrepareMenu();

            Container = UnityEngine.Object.Instantiate(MenuPresets.Menu);
            Container.name = "Configuration menu";
            Container.transform.SetParent(parent, false);
            LifecycleListener lifecycle = Container.AddComponent<LifecycleListener>();

            lifecycle.DestroyEvent += Event_OnDestroy;

            Overlay = Container.transform.Find("Overlay").gameObject;
            ShowMenu = Container.transform.Find("Show menu").gameObject;
            Menu = Container.transform.Find("Menu").gameObject;

            FileName = Menu.FindChild("Info/File name");
            FileText = FileName.FindChild("Area/Text").GetComponent<TextMeshProUGUI>();

            Toggler = new(this, Container);
            Pages = new(this);
            Buttons = new(this);
            Tooltip = new(this);
            Categories = new(this);
            Sections = new(this);
            Configs = new(this);

            // This should not be in the constructor but in the creator

            Pages.UpdateContent();
            Pages.UpdateSelf();

            Categories.UpdateContent();
            Categories.UpdateSelf();

            Sections.UpdateContent();
            Sections.UpdateSelf();

            Configs.UpdateContent();
            Configs.UpdateSelf();

            MenuEventRouter.OnAction_CreateMenu();
        }

        private void Event_OnDestroy()
        {
            Pages.Destroy();
            Tooltip.Destroy();
            _instance = null;
            MenuEventRouter.OnAction_DestroyMenu();
        }

        internal void Destroy()
        {
            UnityEngine.Object.Destroy(Container);
        }
    }
}
