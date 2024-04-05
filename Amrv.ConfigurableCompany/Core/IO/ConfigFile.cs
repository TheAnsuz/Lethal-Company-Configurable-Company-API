using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Utils.IO;
using System.Collections.Generic;
using System.IO;

namespace Amrv.ConfigurableCompany.Core.IO
{
    public sealed class ConfigFile
    {
        public const string METADATA_VERSION = "VERSION";
        public const string FLAG_ENABLED = "enabled";

        private readonly CCFGFile _ccfg;

        public string File => _ccfg.File;

        public ConfigFile(string path, string file, int version, char type)
        {
            _ccfg = new(Path.Combine(path, file));
            _ccfg.AddMetadata(METADATA_VERSION, $"{version}{type}");
        }

        public string Version => _ccfg.Metadata().GetValueOrDefault(METADATA_VERSION, null);

        public bool IsValidVersion(int version, char type)
        {
            if (_ccfg.TryGetMetadata(METADATA_VERSION, out string value))
            {
                return int.TryParse(value[..^1], out int fileVersion) && version >= fileVersion;
            }
            return false;
        }

        public bool WriteConfigValue(CConfig config)
        {
            Dictionary<string, string> values = [];
            if (config.SerializeValue(out string serialized))
            {
                if (config.Toggleable)
                    values[FLAG_ENABLED] = $"{config.Enabled}";
                _ccfg.AddEntry(config.ID, serialized, values);

                return true;
            }

            return false;
        }

        public bool ReadConfigValue(CConfig config)
        {
            if (_ccfg.TryGetEntry(config.ID, out CCFGFile.CCFGEntry entry))
            {
                config.DeserializeValue(entry.Value, ChangeReason.READ_FROM_FILE);

                if (config.Toggleable && entry.Metadata.TryGetValue(FLAG_ENABLED, out string enabledValue))
                    config.Enabled = enabledValue.ToLower().Equals("true");

                return true;
            }
            return false;
        }

        public void Load()
        {
            _ccfg.Read();
        }

        public void Save()
        {
            _ccfg.Write();
        }
    }
}
