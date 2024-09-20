using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Core.Display.menu;
using Amrv.ConfigurableCompany.Plugin;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuConfigs : IMenuPart
    {
        private readonly MenuBind Bind;

        private readonly Dictionary<CConfig, MenuConfig> _configs = new(CConfig.Storage.Count);

        internal MenuConfigs(MenuBind bind)
        {
            Bind = bind;
        }

        public void AddConfig(CConfig config)
        {
            MenuConfig menuConfig = null;

            if (config.Section != null)
                menuConfig = MenuConfig.CreateConfig(Bind.Sections.GetSection(config.Section).Content.transform, config, Bind, false);

            else if (config.Category != null)
                menuConfig = MenuConfig.CreateConfig(Bind.Categories.GetCategory(config.Category).Content.transform, config, Bind, true);

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
            foreach (var entry in _configs.Keys)
            {
                entry.Reset();
            }
        }

        public IEnumerator UpdateContent()
        {
            // I need to somehow optimize this, not sure how but is the second largest time consuming part of the load
            int actual = 0;
            var loader = MenuLoader.GetInstance();
            long lastMs = 0;
            long lastUpdateMs = 0;
#if DEBUG
            int count = CConfig.Storage.Count;
            int notify = count / 10;
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            Stopwatch totalTime = Stopwatch.StartNew();

            foreach (CConfig config in CConfig.Storage.Values)
            {
                AddConfig(config);
                actual++;

                if (totalTime.ElapsedMilliseconds > lastMs + 2500)
                {
                    lastMs = totalTime.ElapsedMilliseconds;
                    ConfigurableCompanyPlugin.Debug($"Configuration loading took more than 2s, delayed by one update");
                    loader.Text = $"Populating menu<br>Updating Configs ({(float)actual / CConfig.Storage.Count:P0})...";
                }

                if (totalTime.ElapsedMilliseconds > lastUpdateMs + 150)
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
    }
}
