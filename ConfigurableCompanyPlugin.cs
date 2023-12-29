using Amrv.ConfigurableCompany.content.model;
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
        public const string PLUGIN_VERSION = "2.0.1";

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
            LethalConfiguration.CreateConfig("dummy_float").SetName("Dummy float").SetSynchronized(true).SetValue(69).SetCategory(category).SetType(ConfigurationTypes.Float).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_percent").SetName("Dummy percent").SetValue(10).SetCategory(category2).SetType(ConfigurationTypes.Percent).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_string").SetName("Dummy string").SetValue("").SetCategory(category2).SetType(ConfigurationTypes.String).SetTooltip("dummy tooltip").Build();
            LethalConfiguration.CreateConfig("dummy_string_small").SetName("Dummy small string").SetValue("")
                .SetTooltip(
                    "Dummy tooltip",
                    "This is another line",
                    "And this line should be long enought to cause an automatic line break in the screen even if its not explicitly marked on the tooltip itself",
                    "A bundh of lines 1",
                    "A bundh of lines 2",
                    "A bundh of lines 3"
                ).SetCategory(category).SetType(ConfigurationTypes.SmallString).Build();
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
            Info(data);
#endif
        }
    }
}
