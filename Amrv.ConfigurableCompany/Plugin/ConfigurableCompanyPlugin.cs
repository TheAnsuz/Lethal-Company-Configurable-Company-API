#if DEBUG
using Amrv.ConfigurableCompany.Plugin.Tests;
#endif
using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core;
using Amrv.ConfigurableCompany.Core.Dependency;
using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.IO;
using Amrv.ConfigurableCompany.Core.Net;
using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Plugin
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(BetterSavesDependency.GUID, BetterSavesDependency.DependencyType)]
    internal sealed class ConfigurableCompanyPlugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "dev.amrv.lethalCompany.config";
        public const string PLUGIN_NAME = "Configurable Company";
        public const string PLUGIN_VERSION = "3.0.1";

        /// <summary>
        /// Plugin folder
        /// </summary>
        public static string PluginFolder { get; private set; }
        /// <summary>
        /// Configurale Company folder in the saves directory
        /// </summary>
        public static string DataFolder { get; private set; }

        private static ConfigurableCompanyPlugin _plugin;

        private readonly Harmony Patcher = new(PLUGIN_GUID);

        public ConfigurableCompanyPlugin()
        {
            if (_plugin != null)
                throw new Exception($"You can't create multiples instances of {nameof(ConfigurableCompanyPlugin)}");
            _plugin = this;

            PluginFolder = Path.GetDirectoryName(base.Info.Location) + Path.DirectorySeparatorChar;
            DataFolder = Path.Combine(Application.persistentDataPath, "Configurable Company", Directory.GetParent(Paths.BepInExRootPath).Name);
            Directory.CreateDirectory(DataFolder);

            Info("Initializating Configurable Company");

            MenuPresets.Ping();
            CTypes.Ping();
            Events.Start();

            NetEventRouter.RegisterListeners();

            Patcher = new(PLUGIN_GUID);

            Patcher.PatchAll(typeof(ConfigurableCompanyPlugin).Assembly);

            DependencyManager.CheckDependencies(Patcher);

#if DEBUG
            foreach (var patch in Patcher.GetPatchedMethods())
            {
                Debug($"Patched {patch.DeclaringType}::{patch.Name}");
            }

            PageBuilding.Ping();
            CategoryBuilding.Ping();
            SectionBuilding.Ping();
            ConfigBuilding.Ping();
#endif

            Application.quitting += OnQuit;

            Info("Completed Configurable Company Initialization");
            LifecycleEventRouter.PluginStart();
        }

        private static void OnQuit()
        {
            IOController.SaveCategories();
            IOController.SaveConfigs();
        }

        internal static void Error(object data)
        {
            _plugin.Logger.LogError(data);
        }

        internal static new void Info(object data)
        {
            _plugin.Logger.LogInfo(data);
        }

#if !DEBUG
        [Obsolete("Only use for debugging purposes")]
#endif
        internal static void Debug(object data)
        {
#if DEBUG
            _plugin.Logger.LogFatal(data);
#endif
        }
    }
}
