using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Amrv.ConfigurableCompany.Core.IO
{
    internal static class IOConfigurations
    {
        private const string FALLBACK_FILE = "LCUnknownFile";
        private static readonly Dictionary<string, CCFGFile> _files = [];

        public static string FileName => GameNetworkManager.Instance?.currentSaveFileName ?? FALLBACK_FILE;
        public static string FullPath => Path.Combine(ConfigurableCompanyPlugin.DataFolder, GameNetworkManager.Instance?.currentSaveFileName ?? FALLBACK_FILE);

        public static string GetFullPath(string file) => Path.Combine(ConfigurableCompanyPlugin.DataFolder, file + ".ccfg");

        public static void Load(string file)
        {
            if (!_files.TryGetValue(file, out CCFGFile cfg))
            {
                cfg = new CCFGFile(GetFullPath(file));
                _files.Add(file, cfg);
            }
            Console.WriteLine($"Loading file {file} from {cfg.File}");
            cfg.Read();
        }

        public static void Save(string file)
        {
            if (_files.TryGetValue(file, out CCFGFile cfg))
            {
                cfg.Write();
            }
        }

        public static void GetOrCreate(string file, out CCFGFile cfg)
        {
            if (!_files.TryGetValue(file, out cfg))
            {
                cfg = new CCFGFile(GetFullPath(file));
                _files.Add(file, cfg);
            }
        }

        public static bool TryGetFile(string file, out CCFGFile cfg) => _files.TryGetValue(file, out cfg);

        internal static void Delete(string file)
        {
            if (_files.TryGetValue(file, out CCFGFile cfg))
            {
                File.Delete(file);
                _files.Remove(file);
            }
        }
    }
}
