using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuConfigs : IMenuPart
    {
        private readonly MenuBind Bind;

        private readonly Dictionary<CConfig, MenuConfig> _configs = [];

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

        public void UpdateContent()
        {
            foreach (CConfig config in CConfig.Storage.Values)
            {
                AddConfig(config);
            }
        }

        public void UpdateSelf()
        {

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
