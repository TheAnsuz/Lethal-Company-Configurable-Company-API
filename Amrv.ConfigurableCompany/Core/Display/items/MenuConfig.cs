using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuConfig
    {
        public static MenuConfig CreateConfig(Transform parent, CConfig config, MenuBind bind, bool first)
        {
            try
            {
                ConfigDisplay display = config.Type.CreateDisplay;
                display.Create(config);
                display.Container.transform.SetParent(parent, false);
                if (first)
                    display.Container.transform.SetAsFirstSibling();
                display.Container.name = $"Config {config.ID}";
                return new MenuConfig(display, bind);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private readonly ConfigDisplay Display;
        private readonly MenuBind Bind;

        private MenuConfig(ConfigDisplay display, MenuBind bind)
        {
            Bind = bind;

            // Generate click callback
            if (!display.Container.TryGetComponent(out Graphic g))
                g = display.Container.AddComponent<NoDrawGraphic>();

            g.raycastTarget = true;
            UIRegion region = display.Container.AddComponent<UIRegion>();

            Display = display;
            Display._resetCallback = OnReset;
            Display._restoreCallback = OnRestore;
            Display._toggleCallback = OnToggle;
            region.OnEnter += OnEnter;
            region.OnExit += OnExit;
        }

        public void Save()
        {
            Display.SaveToConfig(out object value);
            Display.Config.TrySet(value);
        }

        public void Load()
        {
            Display.LoadFromConfig(Display.Config.Value);
        }

        private void OnEnter(PointerEventData e)
        {
            Bind.Tooltip.DisplayedConfig = Display.Config;
        }

        private void OnExit(PointerEventData e)
        {
            if (Bind.Tooltip.DisplayedConfig?.Equals(Display.Config) ?? false)
                Bind.Tooltip.DisplayedConfig = null;
        }

        internal void ReceiveToggle(bool enabled)
        {
            Display.WhenToggled(enabled);
        }

        private void OnToggle(bool enable)
        {
            Display.Config.Enabled = enable;
        }

        internal void ReceiveReset()
        {
            Load();
            Display.WhenReset();
        }

        private void OnReset()
        {
            Display.Config.Reset(ChangeReason.USER_RESET);
        }

        private void OnRestore()
        {
            Load();
            Display.WhenRestored();
        }
    }
}
