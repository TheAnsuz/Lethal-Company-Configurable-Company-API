using Amrv.ConfigurableCompany.Core.Display.menu;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.scripts;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display
{
    internal class MenuBind
    {
        // The game object that contains all the menu
        protected readonly GameObject Container;

        public readonly GameObject Overlay;
        public readonly GameObject ShowMenu;
        public readonly GameObject Menu;

        protected readonly CoroutinePool ThreadPool;
        protected readonly GameObject FileName;
        protected readonly TextMeshProUGUI FileText;
        protected readonly TextMeshProUGUI BetaText;

        public readonly MenuToggle Toggler;
        public readonly MenuPages Pages;
        public readonly MenuButtons Buttons;
        public readonly MenuTooltip Tooltip;
        public readonly MenuCategories Categories;
        public readonly MenuSections Sections;
        public readonly MenuConfigs Configs;
        public readonly MenuPresets Presets;

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
        public static IEnumerator Create(Transform parent)
        {
            if (_instance == null || _instance.Container == null)
            {
                _instance = new(parent);
                yield return null;
                yield return _instance.UpdateMenu();
            }
            else
            {
                ConfigurableCompanyPlugin.Warn("Can't create MenuBind while there is an existing one");
            }

            // Setters
            _instance.Toggler.Open = false;
            _instance.Toggler.Visible = false;
            _instance.Filename = GameNetworkManager.Instance.currentSaveFileName;
            MenuLoader.GetInstance().Fill = 1;

        }

        public static MenuBind GetInstance() => _instance;

        private MenuBind(Transform parent)
        {
            var loader = MenuLoader.GetInstance();

            ConfigurableCompanyPlugin.Debug($"[MenuBind] Preparing menu");

            loader.Text = "Creating menu<br>Preparing menu...";
            loader.Fill = 0.1f;
            MenuEventRouter.OnAction_PrepareMenu();

            ConfigurableCompanyPlugin.Debug($"[MenuBind] Instantiating container");

            loader.Text = "Creating menu<br>Instantiating container...";
            loader.Fill = 0.05f;
            Container = Object.Instantiate(MenuPrefabs.Menu);
            Container.SetActive(false);
            Container.name = "Configuration menu";
            Container.transform.SetParent(parent, false);
            LifecycleListener lifecycle = Container.AddComponent<LifecycleListener>();

            lifecycle.DestroyEvent += Event_OnDestroy;
            loader.Fill = 0.1f;

            Overlay = Container.transform.Find("Overlay").gameObject;
            ShowMenu = Container.transform.Find("Show menu").gameObject;
            Menu = Container.transform.Find("Menu").gameObject;
            loader.Fill = 0.12f;

            FileName = Menu.FindChild("Info/File name");
            FileText = FileName.FindChild("Area/Text").GetComponent<TextMeshProUGUI>();
            loader.Fill = 0.135f;

            Menu.FindChild("Beta").AddComponent<RegionButton>().OnMouseClick += OnIssuesButtonClick;
            Menu.FindChild("Help").GetComponent<Button>().onClick.AddListener(OnHelpButtonClick);
            BetaText = Menu.FindChild("Beta").GetComponent<TextMeshProUGUI>();

            loader.Text = "Creating menu<br>Creating Toggler...";
            loader.Fill = 0.15f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Toggler'");
            Toggler = new(this, Container);

            loader.Text = "Creating menu<br>Creating Pages...";
            loader.Fill = 0.20f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Pages'");
            Pages = new(this);

            loader.Text = "Creating menu<br>Creating Buttons...";
            loader.Fill = 0.24f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Buttons'");
            Buttons = new(this);

            loader.Text = "Creating menu<br>Creating Tooltip...";
            loader.Fill = 0.29f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Tooltip'");
            Tooltip = new(this);

            loader.Text = "Creating menu<br>Creating Categories...";
            loader.Fill = 0.33f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Categories'");
            Categories = new(this);

            loader.Text = "Creating menu<br>Creating Sections...";
            loader.Fill = 0.38f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Sections'");
            Sections = new(this);

            loader.Text = "Creating menu<br>Creating Configs...";
            loader.Fill = 0.42f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Configs'");
            Configs = new(this);

            loader.Text = "Creating menu<br>Creating Presets...";
            loader.Fill = 0.49f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Presets'");
            Presets = new(this);

        }

        private IEnumerator UpdateMenu()
        {
            var loader = MenuLoader.GetInstance();

            loader.Text = "Populating menu<br>Updating Pages...";
            loader.Fill = 0.5f;
            yield return null;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Pages'");
            yield return Pages.UpdateContent();
            loader.Fill = 0.54f;
            yield return null;
            yield return Pages.UpdateSelf();
            loader.Fill = 0.59f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Categories...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Categories'");
            yield return Categories.UpdateContent();
            loader.Fill = 0.62f;
            yield return null;
            yield return Categories.UpdateSelf();
            loader.Fill = 0.66f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Sections...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Sections'");
            yield return Sections.UpdateContent();
            loader.Fill = 0.7f;
            yield return null;
            yield return Sections.UpdateSelf();
            loader.Fill = 0.74f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Configs (0%)...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Configs' 1/2");
            yield return Configs.UpdateContent();
            loader.Fill = 0.85f;
            yield return null;
            loader.Text = "Populating menu<br>Updating Config holder...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Configs' 2/2");
            yield return Configs.UpdateSelf();
            loader.Fill = 0.89f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Presets...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Presets'");
            yield return Presets.UpdateContent();
            loader.Fill = 0.92f;
            yield return null;
            yield return Presets.UpdateSelf();
            loader.Fill = 0.96f;
            yield return null;

            ConfigurableCompanyPlugin.Debug($"[MenuBind] Menu created");

            MenuEventRouter.OnAction_CreateMenu();
            loader.Fill = 0.99f;
            yield return null;
        }

        private void OnHelpButtonClick()
        {
            Application.OpenURL("https://github.com/TheAnsuz/Lethal-Company-Configurable-Company-API/wiki/user_usage");
        }

        private void OnIssuesButtonClick(object sender, PointerEventData e)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(BetaText, e.pointerPressRaycast.worldPosition, null);

            if (linkIndex == -1)
                return;

            Application.OpenURL("https://github.com/TheAnsuz/Lethal-Company-Configurable-Company-API/issues/new/choose");
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
