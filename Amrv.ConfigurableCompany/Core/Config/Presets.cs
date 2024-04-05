using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils.IO;
using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;

namespace Amrv.ConfigurableCompany.Core.Config
{
    public static class Presets
    {
        public const int VERSION = 2;
        public const char TYPE = 'p';

        public const string PRESET_DEFAULT = "Default.ccfg";

        public static string Folder = Path.Combine(Paths.ConfigPath, "ConfigurableCompany", "Presets");

        private static readonly List<string> _presets = [];
        public static readonly IReadOnlyList<string> List = _presets.AsReadOnly();
        public static string Preset { get; internal set; }
        static Presets()
        {
            Update(PRESET_DEFAULT);
            GeneratePrefabList();
        }

        public static void Create(string name)
        {
            if (File.Exists(Path.Combine(Folder, name)))
                return;

            File.Create(Path.Combine(Folder, name)).Close();
            GeneratePrefabList();

            ConfigEventRouter.OnPreset_Create(name);
        }

        public static void Delete(string name)
        {
            if (!File.Exists(Path.Combine(Folder, name)))
                return;

            if (name.Equals(PRESET_DEFAULT))
                return;

            File.Delete(Path.Combine(Folder, name));
            GeneratePrefabList();

            ConfigEventRouter.OnPreset_Delete(name);
        }

        public static void Update(string name)
        {
            var ccfg = new ConfigFile(Folder, name, VERSION, TYPE);

            ccfg.Load();
            foreach (CConfig config in CConfig.Storage.Values)
            {
                ccfg.WriteConfigValue(config);
            }
            ccfg.Save();

            ConfigEventRouter.OnPreset_Update(name);
        }

        public static void Stablish(string name)
        {
            var ccfg = new ConfigFile(Folder, name, VERSION, TYPE);

            ccfg.Load();
            foreach (CConfig config in CConfig.Storage.Values)
            {
                ccfg.ReadConfigValue(config);
            }
            ConfigEventRouter.OnPreset_Stablish(name);
        }

        private static void GeneratePrefabList()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            _presets.Clear();
            foreach (var path in Directory.GetFiles(Folder, "*.ccfg", SearchOption.TopDirectoryOnly))
                _presets.Add(Path.GetFileName(path));
        }
    }
}
