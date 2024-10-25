using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display
{
    internal class MenuBind
    {
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
            _instance.Toggler.Item.Open = false;
            _instance.Toggler.Item.Visible = false;
            _instance.Filename = GameNetworkManager.Instance.currentSaveFileName;
            MenuLoader.GetInstance().Fill = 1;

        }

        public static MenuBind GetInstance() => _instance;

        // The game object that contains all the menu
        protected GameObject Container { get; private set; }

        public GameObject Overlay { get; private set; }
        public GameObject ShowMenu { get; private set; }
        public GameObject Menu { get; private set; }

        protected GameObject FileName { get; private set; }
        protected TextMeshProUGUI FileText { get; private set; }
        protected TextMeshProUGUI BetaText { get; private set; }

        private readonly Reference<MenuBind> CurrentBind = new();

        public readonly Reference<MenuToggle> Toggler = new();
        public readonly Reference<MenuPages> Pages = new();
        public readonly Reference<MenuButtons> Buttons = new();
        public readonly Reference<MenuTooltip> Tooltip = new();
        public readonly Reference<MenuCategories> Categories = new();
        public readonly Reference<MenuSections> Sections = new();
        public readonly Reference<MenuConfigs> Configs = new();
        public readonly Reference<MenuPresets> Presets = new();

        public string Filename
        {
            get => FileText.text;
            set
            {
                FileName.SetActive(!string.IsNullOrEmpty(value));
                FileText.SetText(value);
            }
        }

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
            Container = UnityEngine.Object.Instantiate(MenuPrefabs.Menu);
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

            CurrentBind.Item = this;

            loader.Text = "Creating menu<br>Creating Toggler...";
            loader.Fill = 0.15f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Toggler'");
            Toggler.Item = new(CurrentBind, Container);

            loader.Text = "Creating menu<br>Creating Pages...";
            loader.Fill = 0.20f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Pages'");
            Pages.Item = new(CurrentBind);

            loader.Text = "Creating menu<br>Creating Buttons...";
            loader.Fill = 0.24f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Buttons'");
            Buttons.Item = new(CurrentBind);

            loader.Text = "Creating menu<br>Creating Tooltip...";
            loader.Fill = 0.29f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Tooltip'");
            Tooltip.Item = new(CurrentBind);

            loader.Text = "Creating menu<br>Creating Categories...";
            loader.Fill = 0.33f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Categories'");
            Categories.Item = new(CurrentBind);

            loader.Text = "Creating menu<br>Creating Sections...";
            loader.Fill = 0.38f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Sections'");
            Sections.Item = new(CurrentBind);

            loader.Text = "Creating menu<br>Creating Configs...";
            loader.Fill = 0.42f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Configs'");
            Configs.Item = new(CurrentBind);

            loader.Text = "Creating menu<br>Creating Presets...";
            loader.Fill = 0.49f;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Creating 'Presets'");
            Presets.Item = new(CurrentBind);

        }

        private IEnumerator UpdateMenu()
        {
            var loader = MenuLoader.GetInstance();

            loader.Text = "Populating menu<br>Updating Pages...";
            loader.Fill = 0.5f;
            yield return null;
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Pages'");
            yield return Pages.Item.UpdateContent();
            loader.Fill = 0.54f;
            yield return null;
            yield return Pages.Item.UpdateSelf();
            loader.Fill = 0.59f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Categories...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Categories'");
            yield return Categories.Item.UpdateContent();
            loader.Fill = 0.62f;
            yield return null;
            yield return Categories.Item.UpdateSelf();
            loader.Fill = 0.66f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Sections...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Sections'");
            yield return Sections.Item.UpdateContent();
            loader.Fill = 0.7f;
            yield return null;
            yield return Sections.Item.UpdateSelf();
            loader.Fill = 0.74f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Configs (0%)...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Configs' 1/2");
            yield return Configs.Item.UpdateContent();
            loader.Fill = 0.85f;
            yield return null;
            loader.Text = "Populating menu<br>Updating Config holder...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Configs' 2/2");
            yield return Configs.Item.UpdateSelf();
            loader.Fill = 0.89f;
            yield return null;

            loader.Text = "Populating menu<br>Updating Presets...";
            ConfigurableCompanyPlugin.Debug($"[MenuBind] Updating 'Presets'");
            yield return Presets.Item.UpdateContent();
            loader.Fill = 0.92f;
            yield return null;
            yield return Presets.Item.UpdateSelf();
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

        private void Event_OnDestroy() => MenuEventRouter.OnAction_DestroyMenu();

        internal void Destroy()
        {
            // Menu bind not deleting
            // Menu buttons not deleting
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuBind deletion in progress");
            Toggler.Item.Destroy(); // Deletes OK
            Toggler.Break();
            Pages.Item.Destroy(); // Deletes OK
            Pages.Break();
            Buttons.Item.Destroy();
            Buttons.Break();
            Tooltip.Item.Destroy(); // Deletes OK
            Tooltip.Break();
            Categories.Item.Destroy(); // Deletes OK (childs OK)
            Categories.Break();
            Sections.Item.Destroy(); // Deletes OK (childs OK)
            Sections.Break();
#if DEBUG
            ConfigurableCompanyPlugin.Debug($"[Destroy] Attempting to remove {ConfigDisplay.instances} config displays");
            ConfigurableCompanyPlugin.Debug($"[Destroy] Attempting to remove {MenuConfig.instances} config entries");
#endif
            Configs.Item.Destroy(); // Deletes OK (childs MID)
            Configs.Break();
#if DEBUG
            ConfigurableCompanyPlugin.Debug($"[Destroy] Remaining to remove {ConfigDisplay.instances} config displays");
            ConfigurableCompanyPlugin.Debug($"[Destroy] Remaining to remove {MenuConfig.instances} config entries");
#endif
            Presets.Item.Destroy(); // Deletes OK (childs OK)
            Presets.Break();

            FileText = null;
            BetaText = null;

            UnityEngine.Object.Destroy(Overlay);
            Overlay = null;
            UnityEngine.Object.Destroy(ShowMenu);
            ShowMenu = null;
            UnityEngine.Object.Destroy(Menu);
            Menu = null;
            UnityEngine.Object.Destroy(FileName);
            FileName = null;
            UnityEngine.Object.Destroy(Container);
            Container = null;

            CurrentBind.Break();
            _instance = null;
        }

        ~MenuBind()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuBind deleted");
        }
    }
}
