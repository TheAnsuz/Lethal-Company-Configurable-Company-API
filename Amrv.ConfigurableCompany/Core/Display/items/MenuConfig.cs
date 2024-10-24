using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Display;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Items
{
    internal class MenuConfig
    {
#if DEBUG
        internal static int instances = 0;
        private readonly string configId;
#endif
        public static MenuConfig CreateConfig(Transform parent, CConfig config, Reference<MenuBind> bind, bool first)
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

        internal protected ConfigDisplay Display { get; private set; }
        internal protected readonly Reference<MenuBind> Bind;

        private MenuConfig(ConfigDisplay display, Reference<MenuBind> bind)
        {
            Bind = bind;
#if DEBUG
            instances++;
            configId = display.Config.ID;
#endif
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
            Display.SaveValue(out object value);
            Display.Config.TrySet(value);
        }

        public void Load()
        {
            Display.LoadValue(Display.Config.Value);
        }

        private void OnEnter(PointerEventData e)
        {
            Bind.Item.Tooltip.Item.DisplayedConfig = Display.Config;
        }

        private void OnExit(PointerEventData e)
        {
            if (Bind.Item.Tooltip.Item.DisplayedConfig?.Equals(Display.Config) ?? false)
                Bind.Item.Tooltip.Item.DisplayedConfig = null;
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

        internal void Destroy()
        {
            Display.Destroy();
            Display = null;
        }

#if DEBUG
        ~MenuConfig()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuConfig deleted {configId}");
            instances--;
        }
#endif
    }
}
