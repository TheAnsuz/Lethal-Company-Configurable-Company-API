using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuConfigs : IMenuPart
    {
        protected const int UPDATE_MAX_MS = 24;
        protected const int UPDATE_NOTIFY_MAX_MS = 2500;

#if DEBUG
        internal static int instances = 0;
#endif

        private Reference<MenuBind> Bind;

        private Dictionary<CConfig, MenuConfig> _configs = new(CConfig.Storage.Count);

        internal MenuConfigs(Reference<MenuBind> bind)
        {
#if DEBUG
            instances++;
#endif
            Bind = bind;
        }

        public void AddConfig(CConfig config)
        {
            MenuConfig menuConfig = null;

            if (config.Section != null)
                menuConfig = MenuConfig.CreateConfig(Bind.Item.Sections.Item.GetSection(config.Section).Content.transform, config, Bind, false);

            else if (config.Category != null)
                menuConfig = MenuConfig.CreateConfig(Bind.Item.Categories.Item.GetCategory(config.Category).Content.transform, config, Bind, true);

            if (menuConfig != null)
                _configs[config] = menuConfig;
        }

        public MenuConfig GetConfig(CConfig config)
        {
            return _configs[config];
        }

        public void Refresh(CConfig config)
        {
            if (_configs.TryGetValue(config, out MenuConfig menuConfig))
            {
                menuConfig.Load();
            }
        }

        public void LoadFromConfig()
        {
            foreach (var config in _configs.Values)
            {
                config.Load();
            }
        }

        public void SaveToConfig()
        {
            foreach (var config in _configs.Values)
            {
                config.Save();
            }
        }

        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuConfigs deletion in progress ({_configs.Count} configs)");

            foreach (MenuConfig entry in _configs.Values)
            {
                entry.Destroy();
            }
            _configs.Clear();

            Bind = null;

            _configs = null;
        }

        public IEnumerator UpdateContent()
        {
            // I need to somehow optimize this, not sure how but is the second largest time consuming part of the load
            int actual = 0;
            var loader = MenuLoader.GetInstance();
            long lastMs = 0;
            long lastUpdateMs = 0;
            int count = CConfig.Storage.Count;
#if DEBUG
            int notify = count / 10;
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            Stopwatch totalTime = Stopwatch.StartNew();

            foreach (CConfig config in CConfig.Storage.Values)
            {
                AddConfig(config);
                actual++;

                if (totalTime.ElapsedMilliseconds > lastMs + UPDATE_NOTIFY_MAX_MS)
                {
                    lastMs = totalTime.ElapsedMilliseconds;
                    ConfigurableCompanyPlugin.Debug($"Configuration loading took more than {UPDATE_NOTIFY_MAX_MS}ms [{actual}/{count}]");
                    loader.Text = $"Populating menu<br>Updating Configs ({(float)actual / CConfig.Storage.Count:P0})...";
                }

                if (totalTime.ElapsedMilliseconds > lastUpdateMs + UPDATE_MAX_MS)
                {
                    lastUpdateMs = totalTime.ElapsedMilliseconds;
                    yield return null;
                }
#if DEBUG
                if (actual % notify == 0)
                {
                    ConfigurableCompanyPlugin.Debug($"Updated configs {(float)actual / count:P0} [{actual}/{count}] ({stopwatch.ElapsedMilliseconds / 1000f}s)");

                    stopwatch.Restart();
                }
#endif
            }
            loader.Text = "Populating menu<br>Updating Configs (100%)...";
#if DEBUG
            stopwatch.Stop();
#endif
            totalTime.Stop();
            ConfigurableCompanyPlugin.Debug($"Async configuration loading completed for {CConfig.Storage.Count} entries in {totalTime.ElapsedMilliseconds / 1000f}s");
        }

        public IEnumerator UpdateSelf()
        {
            yield break;
        }

        internal void ReceiveToggle(CConfig config, bool enabled)
        {
            _configs[config].ReceiveToggle(enabled);
        }

        internal void ReceiveReset(CConfig config)
        {
            _configs[config].ReceiveReset();
        }

#if DEBUG
        ~MenuConfigs()
        {
            instances--;
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuConfigs deleted");
        }
#endif
    }
}
