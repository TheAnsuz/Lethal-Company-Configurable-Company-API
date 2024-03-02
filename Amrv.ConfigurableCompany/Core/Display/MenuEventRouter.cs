using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.Config;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Plugin;

namespace Amrv.ConfigurableCompany.Core.Display
{
    internal static class MenuEventRouter
    {
        public static void OnClick_Save()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Save");
            MenuController.SaveConfigs();
            IOController.SetConfigCache();
            IOController.SaveConfigs();
            CEvents.MenuEvents.Save.Invoke();
        }

        public static void OnClick_Reset()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Reset");
            foreach (CConfig config in CConfig.Storage.Values)
                config.Reset(ChangeReason.USER_RESET);
            IOController.SetConfigCache();
            IOController.SaveConfigs();
            CEvents.MenuEvents.Reset.Invoke();
        }

        public static void OnClick_Restore()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Restore");
            MenuController.LoadConfigs();
            CEvents.MenuEvents.Restore.Invoke();
        }

        public static void OnClick_Copy()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Copy");
            Clipboard.CopyToClipboard();
            CEvents.MenuEvents.Copy.Invoke();
        }

        public static void OnClick_Paste()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Paste");
            Clipboard.PasteFromClipboard();
            CEvents.MenuEvents.Paste.Invoke();
        }

        public static void OnClick_ShowPage(CPage page)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | ShowPage ({page.Name})");
            MenuController.SetCurrentPage(page);
            CEvents.MenuEvents.ChangePage.Invoke(new(page));
        }

        public static void OnAction_PrepareMenu()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnAction | Prepare");
            CEvents.MenuEvents.Prepare.Invoke();
        }

        public static void OnAction_CreateMenu()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnAction | Create");
            CEvents.MenuEvents.Create.Invoke();
        }

        public static void OnAction_DestroyMenu()
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnAction | Destroy");
            MenuController.Destroy();
            CEvents.MenuEvents.Destroy.Invoke();
        }

        public static void OnClick_ToggleMenu(bool open)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Toggle ({(open ? "Open" : "Close")})");
            CEvents.MenuEvents.Toggle.Invoke(new(open));
        }

        public static void OnAction_VisibleMenu(bool visible)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnAction | Visible ({(visible ? "Visible" : "Hidden")})");
            CEvents.MenuEvents.Visible.Invoke(new(visible));
        }

        public static void OnAction_ToggleCategory(CCategory category, bool active)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnAction | Toggle Category ({category.ID}, {(active ? "Visible" : "Hidden")})");
            IOController.SetCategoryOpenState(category, active);
        }
    }
}
