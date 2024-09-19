using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.Core.Display.menu;
using Amrv.ConfigurableCompany.Core.Display.scripts;
using DigitalRuby.ThunderAndLightning;
using System.Collections;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display
{
    internal static class MenuController
    {
        private static MenuBind Instance;

        public static void Create(GameObject parent, MenuManager manager)
        {
            GameObject creator = new("Configurable company menu creator", typeof(CoroutinePool));
            CoroutinePool pool = creator.GetComponent<CoroutinePool>();

            pool.StartCoroutine(CreateAsync(parent, manager, pool));
        }

        private static IEnumerator CreateAsync(GameObject parent, MenuManager manager, CoroutinePool creatorPool)
        {
            MenuLoader.GetInstance().Visible = true;
            MenuLoader.GetInstance().Fill = 0;
            MenuLoader.GetInstance().Text = "Creating menu";
            yield return MenuBind.Create(parent.transform);

            Instance = MenuBind.GetInstance();

            MenuLoader.GetInstance().Visible = true;
            MenuLoader.GetInstance().Fill = 1;
            MenuLoader.GetInstance().Text = "Activating";
            yield return null;

            SetVisible(manager.HostSettingsScreen.activeSelf);
            Instance.Toggler.Open = false;

            MenuLoader.GetInstance().Visible = false;

            yield return null;
            Object.Destroy(creatorPool.gameObject, 5f);
        }

        public static bool IsLocked()
        {
            return Instance?.Toggler.Locked ?? false;
        }

        public static void SetLocked(bool locked)
        {
            if (Instance == null) return;

            Instance.Toggler.Locked = locked;
        }

        public static void SetVisible(bool visible)
        {
            if (Instance == null) return;

            Instance.Toggler.Visible = visible;
        }

        public static void SetCurrentFileName(string filename)
        {
            if (Instance == null) return;

            Instance.Filename = filename;
        }

        public static void SetRandomizerDetails(InfoProvider info)
        {
            if (Instance == null) return;

            Instance.Buttons.SetRandomizerDetails(info);
        }

        public static void SetCurrentPage(CPage page)
        {
            if (Instance == null) return;

            Instance.Pages.CurrentPage = page;
        }

        public static void AddPage(CPage page)
        {
            if (Instance == null) return;

            Instance.Pages.AddPage(page);
        }

        public static void AddCategory(CCategory category)
        {
            if (Instance == null) return;

            Instance.Categories.AddCategory(category);
        }

        public static void AddSection(CSection section)
        {
            if (Instance == null) return;

            Instance.Sections.AddSection(section);
        }

        public static void AddConfig(CConfig config)
        {
            if (Instance == null) return;

            Instance.Configs.AddConfig(config);
        }

        public static void RefreshConfig(CConfig config)
        {
            if (Instance == null) return;

            Instance.Configs.Refresh(config);
        }

        public static void SaveConfigs()
        {
            if (Instance == null) return;

            Instance.Configs.SaveToConfig();
        }

        public static void LoadConfigs()
        {
            if (Instance == null) return;

            Instance.Configs.LoadFromConfig();
        }

        public static void UpdateConfig(CConfig config, ChangeReason reason)
        {
            if (Instance == null) return;

            if (reason == ChangeReason.USER_RESET || reason == ChangeReason.SCRIPT_RESET)
                Instance.Configs.ReceiveReset(config);
            else
                Instance.Configs.Refresh(config);
        }

        public static void TriggerToggleConfig(CConfig config, bool enabled)
        {
            if (Instance == null) return;
            Instance.Configs.ReceiveToggle(config, enabled);
        }

        public static void Destroy()
        {
            if (Instance == null) return;

            Instance.Destroy();
            Instance = null;
        }

        public static void RefreshPresets()
        {
            if (Instance == null) return;

            Instance.Presets.UpdateContent();
        }
    }
}
