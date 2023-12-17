using ConfigurableCompany.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model
{
    public sealed class ConfigurationLoader
    {
        private ConfigurationLoader() { }

        public static string DefaultFile => GameNetworkManager.Instance.currentSaveFileName;

        public static void SaveAll() => SaveAll(DefaultFile);
        public static void SaveAll(string filename)
        {
            Console.WriteLine($"Saving to '{filename}'");

            FileDataCache cache = FileDataCache.GetFileCache(filename);

            SaveCategory(ConfigurationCategory.DEFAULT, cache);

            foreach (ConfigurationCategory category in ConfigurationCategory.Registered)
                SaveCategory(category, cache);

            if (cache.RequestSave())
                Console.WriteLine("Saving to disk");
            else
                Console.WriteLine("Saving to cache");
        }

        public static void LoadAll() => LoadAll(DefaultFile);
        public static void LoadAll(string filename)
        {
            Console.WriteLine($"Loading from '{filename}'");

            FileDataCache cache = FileDataCache.GetFileCache(filename);

            if (cache.RequestLoad())
                Console.WriteLine("Loading from disk");
            else
                Console.WriteLine("Loading from cache");

            LoadCategory(ConfigurationCategory.DEFAULT, cache);

            foreach (ConfigurationCategory category in ConfigurationCategory.Registered)
                LoadCategory(category, cache);

        }

        private static void LoadCategory(ConfigurationCategory category, FileDataCache cache)
        {
            foreach (Configuration configuration in category.Configurations)
            {
                configuration.Reset(ChangeReason.RESET);

                if (cache.TryRead(configuration.ID, out string value))
                {
                    //Console.WriteLine($"[LoadCategory] Reading {configuration.ID} : {value}");
                    if (!configuration.Set(value, ChangeReason.LOADED_FROM_FILE))
                    {
                        Console.Error.WriteLine($"Unable to set configuration {configuration.ID} value to {value}");
                    }
                }
            }
        }

        private static void SaveCategory(ConfigurationCategory category, FileDataCache cache)
        {
            foreach (Configuration configuration in category.Configurations)
            {
                Console.WriteLine($"[SaveCategory] Writing {configuration.ID} : {configuration.GetRaw()}");
                cache.Write(configuration.ID, configuration.GetRaw().ToString());
            }
        }
    }
}
