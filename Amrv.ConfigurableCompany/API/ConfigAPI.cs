using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using BepInEx.Configuration;
using System.IO;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API
{
    public static class ConfigAPI
    {
        public const int AUTOGEN_BYTES = 8;

        public const string PLUGIN_GUID = ConfigurableCompanyPlugin.PLUGIN_GUID;
        public const string PLUGIN_VERSION = ConfigurableCompanyPlugin.PLUGIN_VERSION;

        public static CConfigBuilder CreateConfig() => CConfig.Builder();
        public static CCategoryBuilder CreateCategory() => CCategory.Builder();
        public static CPageBuilder CreatePage() => CPage.Builder();
        public static CSectionBuilder CreateSection() => CSection.Builder();

        public static bool TryGetPage(string id, out CPage page) => CPage.Storage.TryGetValue(id, out page);
        public static bool TryGetCategory(string id, out CCategory category) => CCategory.Storage.TryGetValue(id, out category);
        public static bool TryGetSection(string id, out CSection secton) => CSection.Storage.TryGetValue(id, out secton);
        public static bool TryGetConfig(string id, out CConfig config) => CConfig.Storage.TryGetValue(id, out config);

        public static CPage GetOrCreatePageAuto(string name, string description = "")
        {
            using CPageBuilder builder = new();
            builder.Name = name;
            builder.Description = description;
            IDGen gen = new(AUTOGEN_BYTES);
            gen.AddDeterminant(name);
            gen.AddDeterminant(description);
            builder.ID = gen.GetIDBase64();
            return builder.GetOrCreate();
        }
        public static CPage CreatePageAuto(string name, string description = "")
        {
            using CPageBuilder builder = new();
            builder.Name = name;
            builder.Description = description;
            IDGen gen = new(AUTOGEN_BYTES);
            gen.AddDeterminant(name);
            gen.AddDeterminant(description);
            builder.ID = gen.GetIDBase64();
            return builder;
        }

        public static CCategory GetOrCreateCategoryAuto(string name, CPage page, Color? color = null)
        {
            using CCategoryBuilder builder = new();
            builder.Name = name;
            builder.CPage = page;
            builder.Color = color ?? Color.white;
            IDGen gen = new(AUTOGEN_BYTES);
            gen.AddDeterminant(name);
            gen.AddDeterminant(page.ID);
            builder.ID = gen.GetIDBase64();
            return builder.GetOrCreate();
        }

        public static CCategory CreateCategoryAuto(string name, CPage page, Color? color = null)
        {
            using CCategoryBuilder builder = new();
            builder.Name = name;
            builder.CPage = page;
            builder.Color = color ?? Color.white;
            IDGen gen = new(AUTOGEN_BYTES);
            gen.AddDeterminant(name);
            gen.AddDeterminant(page.ID);
            builder.ID = gen.GetIDBase64();
            return builder;
        }

        public static CSection GetOrCreateSectionAuto(string name, CCategory category)
        {
            using CSectionBuilder builder = new();
            builder.Name = name;
            builder.CCategory = category;
            IDGen gen = new(AUTOGEN_BYTES);
            gen.AddDeterminant(name);
            gen.AddDeterminant(category.ID);
            builder.ID = gen.GetIDBase64();
            return builder.GetOrCreate();
        }

        public static CSection CreateSectionAuto(string name, CCategory category)
        {
            using CSectionBuilder builder = new();
            builder.Name = name;
            builder.CCategory = category;
            IDGen gen = new(AUTOGEN_BYTES);
            gen.AddDeterminant(name);
            gen.AddDeterminant(category.ID);
            builder.ID = gen.GetIDBase64();
            return builder;
        }

        public static CConfig ConfigFromBepInEx<T>(ConfigEntry<T> entry)
        {
            string entrySection = entry.Definition.Section;

            // Get or create the page for this configuration
            CPage page = GetOrCreatePageAuto(
                Path.GetFileNameWithoutExtension(
                    entry.ConfigFile.ConfigFilePath),
                entry.ConfigFile.ConfigFilePath
                );
            CCategory category;
            CSection section = null;

            // Get or create both the category and section of this configuration
            int split = entrySection.IndexOf('.');
            if (split == -1)
                category = GetOrCreateCategoryAuto(entrySection, page);
            else
            {
                category = GetOrCreateCategoryAuto(entrySection[..split], page);
                section = GetOrCreateSectionAuto(entrySection[split..], category);
            }

            // Create the configuration
            using CConfigBuilder builder = new();
            builder.SetName(entry.Definition.Key);
            builder.SetCategory(category);
            builder.SetSection(section);
            builder.SetExperimental(true);
            builder.SetTooltip(entry.Description.Description);
            builder.Name = entry.Definition.Key;

            if (section != null)
                builder.CSection = section;
            else
                builder.CCategory = category;

            // Adjust custom tags to make the config have my own tags
            foreach (object tag in entry.Description.Tags)
                switch (tag.ToString().ToLower())
                {
                    case "experimental":
                        builder.SetExperimental(true);
                        break;
                    case "synchronized":
                        builder.SetSynchronized(true);
                        break;
                    case "toggleable":
                        builder.SetToggleable(true);
                        break;
                    case "disabled":
                        builder.SetEnabled(false);
                        break;
                }

            builder.SetDefaultValue(entry.DefaultValue);
            builder.SetValue(entry.Value);

            return builder.Build();
        }
    }
}
