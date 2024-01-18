using Amrv.ConfigurableCompany.content.model.data;
using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class CategoryIO
    {
        private CategoryIO() { }

        public static readonly string CategoryFile = FileUtils.FILE_PATH + Path.DirectorySeparatorChar + "CategoryState";

        private static readonly Dictionary<ConfigurationCategory, bool> _categoryOpenState = new();

        public static bool ShouldBeOpen(ConfigurationCategory Category)
        {
            return _categoryOpenState.TryGetValue(Category, out bool open) && open;
        }

        public static void ShouldBeOpen(ConfigurationCategory Category, bool open)
        {
            Console.WriteLine($"Saved open state for {Category.ID} set to {open}");
            _categoryOpenState[Category] = open;
        }

        public static void ReadAll()
        {
            ConfigurableCompanyPlugin.Info($"Reading all category states from file {CategoryFile}");

            if (!File.Exists(CategoryFile))
                return;

            ConfigurationBundle categoryStateBundle = new DefaultConfigurationBundle();
            string content = "";

            try
            {
                content = File.ReadAllText(CategoryFile);
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to read category state from file {CategoryFile}", ex);
            }

            try
            {
                categoryStateBundle.Unpack(content);
            }
            catch (Exception ex)
            {
                try
                {
                    File.Delete(CategoryFile);
                }
                catch (Exception ex2)
                {
                    throw new IOException($"Unable to remove invalid category state file", ex2);
                }
                throw new IOException($"Unable to unpack category state bundle", ex);
            }

            foreach (ConfigurationCategory category in ConfigurationCategory.Categories)
            {
                if (categoryStateBundle.TryGet(category.ID, out string openString))
                {
                    Console.Error.WriteLine($"{category.ID} read open state [{openString.ToLower().Equals("Open")}]");
                    ShouldBeOpen(category, openString.ToLower().Equals("open"));
                }
            }
        }

        public static void SaveAll()
        {
            ConfigurableCompanyPlugin.Info($"Saving all category states to file {CategoryFile}");

            ConfigurationBundle bundle = new DefaultConfigurationBundle();

            // Get all the contents of the file so configs that are not currently loaded dont get deleted
            string existingContent;
            try
            {
                existingContent = File.Exists(CategoryFile) ? File.ReadAllText(CategoryFile) : "";

                bundle.Unpack(existingContent);
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to read category state from file {CategoryFile}", ex);
            }
            finally
            {
                // Save current configs replacing those that already exist
                foreach (var pair in _categoryOpenState)
                {
                    bundle.Add(pair.Key.ID, pair.Value ? "Open" : "Closed");
                }

                try
                {
                    string content = bundle.Pack();

                    File.WriteAllText(CategoryFile, content);
                }
                catch (Exception ex)
                {
                    throw new IOException($"Can't save category state to file {CategoryFile}", ex);
                }
            }
        }
    }
}
