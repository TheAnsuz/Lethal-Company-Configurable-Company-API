using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.Core.Config;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Plugin;
using System.Collections;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display
{
    internal static class MenuController
    {
        public delegate IEnumerator AfterCreationProcess(MenuManager manager);

        private static MenuBind Instance;

        private static GameObject _creator;
        private static CoroutinePool _pool;

        private static AfterCreationProcess _afterCreation;
        public static void AfterCreation(AfterCreationProcess action) => _afterCreation += action;

        public static void Create(GameObject parent, MenuManager manager)
        {
            _creator = new("Configurable company menu creator", typeof(CoroutinePool));
            _pool = _creator.GetComponent<CoroutinePool>();

            _pool.StartCoroutine(CreateAsync(parent, manager, _pool));
        }

        public static void DestroyIfCreating()
        {
            if (_pool != null)
            {
                _pool.StopAllCoroutines();
                _pool = null;
            }
            Object.Destroy(_creator);
            _creator = null;
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
            Instance.Toggler.Item.Open = false;

            MenuLoader.GetInstance().Visible = false;

            if (_afterCreation != null)
                yield return _afterCreation.Invoke(manager);

            yield return null;
            Object.Destroy(creatorPool.gameObject, 5f);
        }

        public static bool IsLocked()
        {
            return Instance?.Toggler.Item.Locked ?? false;
        }

        public static void SetLocked(bool locked)
        {
            if (Instance == null) return;

            Instance.Toggler.Item.Locked = locked;
        }

        public static void SetVisible(bool visible)
        {
            if (Instance == null) return;

            Instance.Toggler.Item.Visible = visible;
        }

        public static void SetCurrentFileName(string filename)
        {
            if (Instance == null) return;

            Instance.Filename = filename;
        }

        public static void SetRandomizerDetails(InfoProvider info)
        {
            if (Instance == null) return;

            Instance.Buttons.Item.SetRandomizerDetails(info);
        }

        public static void SetCurrentPage(CPage page)
        {
            if (Instance == null) return;

            Instance.Pages.Item.CurrentPage = page;
        }

        public static void AddPage(CPage page)
        {
            if (Instance == null) return;

            Instance.Pages.Item.AddPage(page);
        }

        public static void AddCategory(CCategory category)
        {
            if (Instance == null) return;

            Instance.Categories.Item.AddCategory(category);
        }

        public static void AddSection(CSection section)
        {
            if (Instance == null) return;

            Instance.Sections.Item.AddSection(section);
        }

        public static void AddConfig(CConfig config)
        {
            if (Instance == null) return;

            Instance.Configs.Item.AddConfig(config);
        }

        public static void RefreshConfig(CConfig config)
        {
            if (Instance == null) return;

            Instance.Configs.Item.Refresh(config);
        }

        public static void SaveConfigs()
        {
            if (Instance == null) return;

            Instance.Configs.Item.SaveToConfig();
        }

        public static void LoadConfigs()
        {
            if (Instance == null) return;

            Instance.Configs.Item.LoadFromConfig();
        }

        public static void UpdateConfig(CConfig config, ChangeReason reason)
        {
            if (Instance == null) return;

            if (reason == ChangeReason.USER_RESET || reason == ChangeReason.SCRIPT_RESET)
                Instance.Configs.Item.ReceiveReset(config);
            else
                Instance.Configs.Item.Refresh(config);
        }

        public static void TriggerToggleConfig(CConfig config, bool enabled)
        {
            if (Instance == null) return;
            Instance.Configs.Item.ReceiveToggle(config, enabled);
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

            Instance.Presets.Item.UpdateContentFull();
        }

        public static void UpdateSeedFromCache()
        {
            ConfigurableCompanyPlugin.Debug($"Trying to update seed info with cached seed");

            if (Instance == null) return;

            if (CCache.UsedSeed == null)
                ConfigurableCompanyPlugin.Debug($"Invalid cached seed for update: {CCache.UsedSeed}");
            else
            {
                InfoProvider info = InfoProvider.Create(CCache.UsedSeed, SpecialSeed.GetSpecialSeed(CCache.UsedSeed));
                SetRandomizerDetails(info);
                ConfigurableCompanyPlugin.Debug($"Updated seed from {(info.IsSpecialSeed ? "Special seed" : info.IsChallenge ? "Challenge seed" : "Normal seed")}: {info.SeedString}");
            }
        }
    }
}
