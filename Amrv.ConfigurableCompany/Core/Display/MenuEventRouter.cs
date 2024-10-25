using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Data;
using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.Config;
using Amrv.ConfigurableCompany.Core.Display.Menu;
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

        public static void OnClick_Randomize(string seed)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Randomize ({seed})");

            if (seed == null || seed == "")
            {
                foreach (var config in CConfig.Storage.Values)
                    config.Reset(ChangeReason.USER_RANDOMIZED);

                CEvents.MenuEvents.Randomize.Invoke(new(RNGProvider.Static, InfoProvider.Default));

                CCache.UsedSeed = "";
                MenuController.SetRandomizerDetails(InfoProvider.Default);
                return;
            }

            InfoProvider info = InfoProvider.Create(seed, SpecialSeed.GetSpecialSeed(seed));
            RNGProvider random = info.IsSpecialSeed ? new RNGProvider(info.SpecialSeed.Seed) : new RNGProvider(RandomSeedParser.FromSeed(seed));

            foreach (var config in CConfig.Storage.Values)
                config.Randomize(random, info, ChangeReason.USER_RANDOMIZED);

            CCache.UsedSeed = info.SeedString;
            MenuController.SetRandomizerDetails(info);
            CEvents.MenuEvents.Randomize.Invoke(new(random, info));
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
            CEvents.MenuEvents.Destroy.Invoke();
        }

        public static void OnClick_ToggleMenu(bool open)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Toggle ({(open ? "Open" : "Close")})");
            CEvents.MenuEvents.Toggle.Invoke(new(open));
        }

        public static void OnClick_PresetCreate(string name)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Preset create | name: {name}");
            Presets.Create(name);
        }

        public static void OnClick_PresetLoad(string name)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Preset load | name: {name}");
            Presets.Stablish(name);
        }

        public static void OnClick_PresetSave(string name)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Preset save | name: {name}");
            //IOController.SetConfigCache();
            //MenuController.SaveConfigs();
            Presets.Update(name);
        }

        public static void OnClick_PresetDelete(string name)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnClick | Preset remove | name: {name}");
            Presets.Delete(name);
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

        public static void OnAction_ToggleSection(CSection section, bool active)
        {
            ConfigurableCompanyPlugin.Debug($"MenuEventRouter > OnAction | Toggle Section ({section.ID}, {(active ? "Visible" : "Hidden")})");
            IOController.SetSectionOpenState(section, active);
        }
    }
}
