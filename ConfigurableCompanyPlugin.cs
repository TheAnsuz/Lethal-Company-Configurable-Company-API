﻿#if DEBUG
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.events;
using Amrv.ConfigurableCompany.content.model.types;
#endif
using Amrv.ConfigurableCompany.content.patch;
using BepInEx;
using HarmonyLib;
using System;

namespace Amrv.ConfigurableCompany
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    internal sealed class ConfigurableCompanyPlugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "dev.amrv.lethalCompany.config";
        public const string PLUGIN_NAME = "Configurable Company";
        public const string PLUGIN_VERSION = "2.5.1";

        private static ConfigurableCompanyPlugin _plugin;

        private Harmony Patcher = new(PLUGIN_GUID);

        public ConfigurableCompanyPlugin()
        {
            if (_plugin != null)
                throw new Exception($"You can't create multiples instances of {nameof(ConfigurableCompanyPlugin)}");

            _plugin = this;

            Initiate();
            Setup();
            Enable();

            Info($"Configurable company plugin loaded!");
        }

        private void Initiate()
        {
            Info("Initializating Configurable Company");

            Patcher = new(PLUGIN_GUID);

            Events.PluginInitialized.Invoke(EventArgs.Empty);
        }

        private void Setup()
        {
            Info("Setting up Configurable Company");

            Patcher.PatchAll(typeof(DisplayConfigurationPatch));
            Patcher.PatchAll(typeof(SteamLobbyPatch));

            Events.PluginSetup.Invoke(EventArgs.Empty);
        }

        private void Enable()
        {
            Info("Enabling Configurable Company");
#if DEBUG

            /*
            foreach (var val in typeof(LevelWeatherType).GetEnumValues())
                Console.WriteLine($"LevelWeatherType::{val}[{(int)val}]");
            Events.ConfigurationChanged.AddListener(delegate (object self, ConfigurationChanged evt)
            {
                Console.WriteLine($"New {evt.Configuration.ID} value is {evt.Configuration.Get(-1)}, {evt.Configuration.Get<LevelWeatherType>()}, {evt.Configuration.Get("nil")}, {evt.Configuration.Get(false)}, {evt.Configuration.Get(10f)}, {evt.Configuration.Get(24l)}");
            });
            */

            //LethalConfiguration.CreateConfig("dummy_other_string").SetName("Dummy other string").SetValue("Custom string value for testing ]]").SetType(ConfigurationTypes.String).SetTooltip("dummy tooltip").Build();
            ConfigurationCategory category = LethalConfiguration.CreateCategory("dummy_category").SetName("Dummy category").SetColorRGB(255, 100, 100);
            ConfigurationCategory category2 = LethalConfiguration.CreateCategory("dummy_category_long_name").SetName("Dummy category with a long name");
            ConfigurationCategory category3 = LethalConfiguration.CreateCategory()
                .SetID("amrv_configurable-company_category-3")
                .SetName("Category 3")
                .SetColorRGB(255, 0, 0)
                .HideIfEmpty(false)
                .Build();
            LethalConfiguration.CreateCategory("dummy_empty_category_autohide").SetName("Should not be visible").HideIfEmpty().Build();
            LethalConfiguration.CreateCategory("dummy_empty_category_nohide").SetName("Should be visible").HideIfEmpty(false).Build();
            Configuration someConfig = LethalConfiguration.CreateConfig("dummy_boolean_2").SetName("Dummy boolean").SetValue(false).SetType(ConfigurationTypes.Boolean).SetTooltip("").Build();
            LethalConfiguration.CreateConfig("dummy_boolean").SetName("Dummy boolean").SetValue(false).SetType(ConfigurationTypes.Boolean).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_integer").SetName("Dummy integer").SetValue(1).SetType(ConfigurationTypes.Integer).SetTooltip($"Dummy integer {new string('*', 50)}").Build();
            LethalConfiguration.CreateConfig("dummy_integer_range").SetName("Dummy integer range").SetValue(1).SetType(ConfigurationTypes.RangeInteger(1, 55)).SetTooltip($"Dummy integer {new string('*', 50)}").Build();
            LethalConfiguration.CreateConfig("dummy_float").SetName("Dummy float").SetSynchronized(true).SetValue(69).SetCategory(category).SetType(ConfigurationTypes.Float).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_float_range").SetName("Dummy float range").SetSynchronized(true).SetValue(69).SetCategory(category).SetType(ConfigurationTypes.RangeFloat(1.25f, 50f)).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_percent").SetName("Dummy percent").SetValue(10).SetCategory(category2).SetType(ConfigurationTypes.Percent).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_string").SetName("Dummy string of N length").SetValue("").SetCategory(category2).SetType(ConfigurationTypes.StringOfLength(199)).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_string_small").SetName("Dummy small string").SetValue("")
                .SetTooltip(
                    "Dummy tooltip",
                    "This is another line",
                    "And this line should be long enought to cause an automatic line break in the screen even if its not explicitly marked on the tooltip itself",
                    "A bundh of lines 1",
                    "A bundh of lines 2",
                    "A bundh of lines 3"
                ).SetCategory(category)
                .SetNeedsRestart(true)
                .SetType(ConfigurationTypes.SmallString).Build();

            LethalConfiguration.CreateConfig("dummy_enum")
                .SetValue(LevelWeatherType.Rainy)
                .SetType(ConfigurationTypes.Options(typeof(LevelWeatherType)))
                .SetName("LevelWeatherType enum")
                .SetCategory(category)
                .Build();

            LethalConfiguration.CreateConfig("dummy_enum_2")
                .SetValue("Texto de ejemplo largo 3")
                .SetType(ConfigurationTypes.Options("Texto de ejemplo largo 1", "Texto de ejemplo largo 2", "Texto de ejemplo largo 3", "Texto de ejemplo largo 4"))
                .SetName("LevelWeatherType enum")
                .SetCategory(category)
                .Build();
            LethalConfiguration.CreateConfig("dummy_percent_needs_restart")
                .SetName("Requires restart")
                .SetType(ConfigurationTypes.Slider(5, 500))
                .SetNeedsRestart(true)
                .Build();
            LethalConfiguration.CreateConfig("dummy_string_small_tooltip_testing")
                .SetName("Dummy small string with an incredible long name that makes no sense at all but needs to be checked")
                .SetValue("")
                .SetCategory(category)
                .SetType(ConfigurationTypes.SmallString)
                .SetTooltip(
                    "Dummy tooltip",
                    "This is another line",
                    "And this line should be long enought to cause an automatic line break in the screen even if its not explicitly marked on the tooltip itself",
                    "A bundh of lines 1",
                    "A bundh of lines 2",
                    "A bundh of lines 3",
                    "A bundh of lines 4",
                    "A bundh of lines 5",
                    "A bundh of lines 6",
                    "A bundh of lines 6",
                    "A bundh of lines 7",
                    "A bundh of lines 1"
                )
                .Build();

            LethalConfiguration.CreateConfig()
                .SetID("amrv_configurable-company_custom-configuration")
                .SetName("Custom configuration")
                .SetTooltip(
                "This is a custom configuration that does nothing",
                "This is a second line for the tooltip",
                "",
                "This is another line")
                .SetCategory(category)
                .SetType(ConfigurationTypes.String)
                .SetValue("Random value")
                .SetExperimental(false)
                .SetSynchronized(false)
                .Build();

            ConfigurationPage page = LethalConfiguration.CreatePage().SetName("Page 2").Build();
            ConfigurationCategory pageCategory = LethalConfiguration.CreateCategory("dummy_page_category").SetName("Dummy page category").SetPage(page).SetColorRGB(255, 255, 0, 0);

            for (int i = 0; i < 5; i++)
            {
                ConfigurationCategory cat = LethalConfiguration.CreateCategory($"dummy_category_{i}").SetName($"Dummy category {i}").SetColorRGB((byte)(i / 50f * 255), 100, 100).Build();
                LethalConfiguration.CreateConfig($"dummy_page_boolean_2_forcat_{i}").SetName("Dummy boolean").SetValue(false).SetCategory(pageCategory).SetType(ConfigurationTypes.Boolean).SetTooltip("dummy tooltip").Build();
                LethalConfiguration.CreateConfig($"dummy_page_boolean_forcat_{i}").SetName("Dummy boolean").SetValue(false).SetCategory(pageCategory).SetType(ConfigurationTypes.Boolean).SetTooltip("dummy tooltip").Build();
                LethalConfiguration.CreateConfig($"dummy_page_integer_forcat_{i}").SetName("Dummy integer").SetValue(1).SetCategory(pageCategory).SetType(ConfigurationTypes.Integer).SetTooltip($"Dummy integer {new string('*', 50)}").Build();
                LethalConfiguration.CreateConfig($"dummy_page_float_forcat_{i}").SetName("Dummy float").SetSynchronized(true).SetValue(69).SetCategory(pageCategory).SetType(ConfigurationTypes.Float).SetTooltip("dummy tooltip").Build();
                LethalConfiguration.CreateConfig($"dummy_page_percent_forcat_{i}").SetName("Dummy percent").SetValue(10).SetCategory(pageCategory).SetType(ConfigurationTypes.Percent).SetTooltip("dummy tooltip").Build();
                LethalConfiguration.CreateConfig($"dummy_page_string_forcat_{i}").SetName("Dummy string").SetValue("").SetCategory(pageCategory).SetType(ConfigurationTypes.String).SetTooltip("dummy tooltip").Build();
                LethalConfiguration.CreateConfig($"dummy_page_string_small_forcat_{i}").SetName("Dummy small string").SetValue("").SetCategory(pageCategory).SetType(ConfigurationTypes.SmallString).SetTooltip("dummy tooltip").Build();
            }

            Events.AfterMenuDisplay += delegate
            {
                for (int i = 0; i < 5; i++)
                {
                    ConfigurationCategory cat = LethalConfiguration.CreateCategory($"dummy_category_{i}").SetName($"Dummy category {i}").SetColorRGB((byte)(i / 50f * 255), 100, 100).Build();
                    LethalConfiguration.CreateConfig($"dummy_boolean_2_forcat_{i}").SetName("Dummy boolean").SetValue(false).SetCategory(cat).SetType(ConfigurationTypes.Boolean).SetTooltip("dummy tooltip").Build();
                    LethalConfiguration.CreateConfig($"dummy_boolean_forcat_{i}").SetName("Dummy boolean").SetValue(false).SetCategory(cat).SetType(ConfigurationTypes.Boolean).SetTooltip("dummy tooltip").Build();
                    LethalConfiguration.CreateConfig($"dummy_integer_forcat_{i}").SetName("Dummy integer").SetValue(1).SetCategory(cat).SetType(ConfigurationTypes.Integer).SetTooltip($"Dummy integer {new string('*', 50)}").Build();
                    LethalConfiguration.CreateConfig($"dummy_float_forcat_{i}").SetName("Dummy float").SetSynchronized(true).SetValue(69).SetCategory(cat).SetType(ConfigurationTypes.Float).SetTooltip("dummy tooltip").Build();
                    LethalConfiguration.CreateConfig($"dummy_percent_forcat_{i}").SetName("Dummy percent").SetValue(10).SetCategory(cat).SetType(ConfigurationTypes.Percent).SetTooltip("dummy tooltip").Build();
                    LethalConfiguration.CreateConfig($"dummy_string_forcat_{i}").SetName("Dummy string").SetValue("").SetCategory(cat).SetType(ConfigurationTypes.String).SetTooltip("dummy tooltip").Build();
                    LethalConfiguration.CreateConfig($"dummy_string_small_forcat_{i}").SetName("Dummy small string").SetValue("").SetCategory(cat).SetType(ConfigurationTypes.SmallString).SetTooltip("dummy tooltip").Build();
                }
            };
#endif
            Events.PluginEnabled.Invoke(EventArgs.Empty);
        }

        internal static void Error(object data)
        {
            if (_plugin == null)
                Console.Error.WriteLine($"[Configurable Company] " + data);
            else
                _plugin.Logger.LogError(data);
        }

        internal static new void Info(object data)
        {
            if (_plugin == null)
                Console.WriteLine($"[Configurable Company] " + data);
            else
                _plugin.Logger.LogInfo(data);
        }

        internal static void Debug(object data)
        {
#if DEBUG
            if (_plugin == null)
                Console.Error.WriteLine($"[Configurable Company DEBUG] {data}");
            else
                _plugin.Logger.LogFatal(data);
#endif
        }
    }
}
