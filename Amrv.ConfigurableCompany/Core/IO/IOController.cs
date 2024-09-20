using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Utils.IO;

namespace Amrv.ConfigurableCompany.Core.IO
{
    public static class IOController
    {
        public const string VERSION = "2";
        public const string FIELD_CUSTOM_SEED = "Seed";

        public static void SaveCategories() => IOCategories.Save();
        public static void LoadCategories() => IOCategories.Load();
        public static void SaveSections() => IOSections.Save();
        public static void LoadSections() => IOSections.Load();

        public static bool GetCategoryOpenState(CCategory category)
        {
            return IOCategories.GetOpenState(category);
        }

        public static void SetCategoryOpenState(CCategory category, bool open)
        {
            IOCategories.SetOpenState(category, open);
        }

        public static bool GetSectionOpenState(CSection section)
        {
            return IOSections.GetOpenState(section);
        }

        public static void SetSectionOpenState(CSection section, bool open)
        {
            IOSections.SetOpenState(section, open);
        }

        public static void SaveConfigs() => IOConfigurations.Save(IOConfigurations.FileName);
        public static void LoadConfigs() => IOConfigurations.Load(IOConfigurations.FileName);
        public static void RemoveConfigs() => IOConfigurations.Delete(IOConfigurations.FileName);

        public static void SetConfigCache(CConfig config)
        {
            IOConfigurations.GetOrCreate(IOConfigurations.FileName, out CCFGFile cfg);

            ApplyMetadata(cfg);

            WriteConfigEntryCache(config, cfg);
        }

        public static bool GetConfigCache(CConfig config)
        {
            if (IOConfigurations.TryGetFile(IOConfigurations.FileName, out CCFGFile cfg))
            {
                if (cfg.TryGetEntry(config.ID, out CCFGFile.CCFGEntry entry))
                {
                    ReadConfigEntryCache(config, entry);
                }
            }
            return false;
        }

        public static void SetConfigMetadata()
        {
            IOConfigurations.GetOrCreate(IOConfigurations.FileName, out CCFGFile cfg);
            ApplyMetadata(cfg);
        }

        public static void SetConfigCache()
        {
            IOConfigurations.GetOrCreate(IOConfigurations.FileName, out CCFGFile cfg);

            ApplyMetadata(cfg);

            foreach (CConfig config in CConfig.Storage.Values)
                WriteConfigEntryCache(config, cfg);
        }

        public static void GetConfigCache()
        {
            if (IOConfigurations.TryGetFile(IOConfigurations.FileName, out CCFGFile cfg))
            {
                if (cfg.TryGetMetadata(FIELD_CUSTOM_SEED, out string seed) && seed != null)
                    CCache.UsedSeed = seed;
                else
                    CCache.UsedSeed = "";

                foreach (CConfig config in CConfig.Storage.Values)
                {
                    if (cfg.TryGetEntry(config.ID, out CCFGFile.CCFGEntry entry))
                    {
                        ReadConfigEntryCache(config, entry);
                    }
                    else
                        config.Reset(ChangeReason.READ_FROM_FILE);
                }
            }
            else
            {
                foreach (CConfig config in CConfig.Storage.Values)
                    config.Reset(ChangeReason.READ_FROM_FILE);
            }
        }

        private static void ApplyMetadata(CCFGFile cfg)
        {
            cfg.AddMetadata(nameof(VERSION), VERSION);

            if (CCache.UsedSeed != null)
                cfg.AddMetadata(FIELD_CUSTOM_SEED, CCache.UsedSeed);
        }

        private static void ReadConfigEntryCache(CConfig config, CCFGFile.CCFGEntry entry)
        {
            config.DeserializeValue(entry.Value, ChangeReason.READ_FROM_FILE);

            if (entry.Metadata.TryGetValue("enabled", out string enabledString))
                config.Enabled = enabledString.ToLower().Equals("true");
        }

        private static void WriteConfigEntryCache(CConfig config, CCFGFile file)
        {
            if (config.SerializeValue(out string serialized))
                file.AddEntry(config.ID, serialized, new()
                {
                    ["enabled"] = config.Enabled.ToString()
                });
        }
    }
}
