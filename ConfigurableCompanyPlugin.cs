using BepInEx;
using BepInEx.Logging;
using ConfigurableCompany.display.item;
using ConfigurableCompany.model;
using ConfigurableCompany.patch;
using ConfigurableCompany.utils;
using HarmonyLib;
using HarmonyLib.Tools;
using System;
using System.Collections.Generic;

namespace ConfigurableCompany
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    internal sealed class ConfigurableCompanyPlugin : BaseUnityPlugin
    {
        public static event EventHandler OnInitiate;
        public static event EventHandler OnSetup;
        public static event EventHandler OnEnable;

        public const string PLUGIN_GUID = "dev.amrv.lethalCompany.config";
        public const string PLUGIN_NAME = "Configurable Company";
        public const string PLUGIN_VERSION = "1.2.0";

        private static ConfigurableCompanyPlugin _plugin;

        private Harmony Patcher = new(PLUGIN_GUID);

        public ConfigurableCompanyPlugin()
        {
            _plugin = this;

            Initiate();
            Setup();
            Enable();

            Logger.LogInfo($"Configurable company plugin loaded!");
        }

        private void Initiate()
        {
            Console.WriteLine("Initializating Configurable Company");
            Patcher = new(PLUGIN_GUID);

            OnInitiate?.Invoke(this, EventArgs.Empty);
        }

        private void Setup()
        {
            Console.WriteLine("Setting up Configurable Company");
            Patcher.PatchAll(typeof(DisplayConfigurationPatch));
            Patcher.PatchAll(typeof(SteamLobbyPatch));

            OnSetup?.Invoke(this, EventArgs.Empty);
        }

        private void Enable()
        {
            Console.WriteLine("Enabling Configurable Company");
#if DEBUG
            ConfigurationBuilder.NewConfig("dummy_other_string").SetName("Dummy other string").SetValue("").SetType(ConfigurationType.String).SetTooltip("dummy tooltip").Build();
            ConfigurationCategory category = ConfigurationBuilder.NewCategory("dummy_category").SetName("Dummy category").Build();
            ConfigurationBuilder.NewConfig("dummy_boolean").SetName("Dummy boolean").SetValue(false).SetCategory(category).SetType(ConfigurationType.Boolean).SetTooltip("dummy tooltip").Build();
            ConfigurationBuilder.NewConfig("dummy_integer").SetName("Dummy integer").SetValue(1).SetCategory(category).SetType(ConfigurationType.Integer).SetTooltip($"Dummy integer {new string('*', 50)}").Build();
            ConfigurationBuilder.NewConfig("dummy_float").SetName("Dummy float").SetSyncronized(true).SetValue(1).SetCategory(category).SetType(ConfigurationType.Float).SetTooltip("dummy tooltip").Build();
            ConfigurationBuilder.NewConfig("dummy_percent").SetName("Dummy percent").SetValue(10).SetCategory(category).SetType(ConfigurationType.Percent).SetTooltip("dummy tooltip").Build();
            ConfigurationBuilder.NewConfig("dummy_string").SetName("Dummy string").SetValue("").SetCategory(category).SetType(ConfigurationType.String).SetTooltip("dummy tooltip").Build();
#endif
            OnEnable?.Invoke(this, EventArgs.Empty);
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
