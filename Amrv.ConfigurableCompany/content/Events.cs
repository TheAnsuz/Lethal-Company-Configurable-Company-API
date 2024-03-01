using Amrv.ConfigurableCompany.API.Event;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.data;
using Amrv.ConfigurableCompany.content.model.events;
using System;
namespace Amrv.ConfigurableCompany
{
    [Obsolete("Use CEvents")]
    public class Events
    {
        private Events() { }

        internal static void Start()
        {
            CEvents.ConfigEvents.ChangeConfig.AddListener(delegate (CEventChangeConfig e)
            {
                ChangeResult result;
                if (e.Success)
                    if (e.Converted)
                        result = ChangeResult.SUCCESS_CONVERTED;
                    else
                        result = ChangeResult.SUCCESS;
                else
                    result = ChangeResult.FAILED;

                if (Configuration.TryGet(e.Config.ID, out Configuration value))
                    ConfigurationChanged.Invoke(new(value, e.OldValue, e.NewValue, e.Reason.OldReason(), result));
            });

            CEvents.ConfigEvents.CreateConfig.AddListener(delegate (CEventCreateConfig e)
            {
                if (Configuration.TryGet(e.Config.ID, out Configuration value))
                    ConfigurationCreated.Invoke(new(value));
            });

            CEvents.LifecycleEvents.PluginStart.AddListener(delegate
            {
                PluginSetup.Invoke(EventArgs.Empty);
                PluginInitialized.Invoke(EventArgs.Empty);
                PluginEnabled.Invoke(EventArgs.Empty);
            });

            CEvents.MenuEvents.Create.AddListener(delegate
            {
                AfterMenuDisplay.Invoke(EventArgs.Empty);
            });

            CEvents.MenuEvents.Prepare.AddListener(delegate
            {
                BeforeMenuDisplay.Invoke(EventArgs.Empty);
            });
        }

        /// <summary>
        /// Event that triggers every time a configuration value has changed.
        /// </summary>
        [Obsolete("Use CEvents.ConfigEvents.ChangeConfig")]
        public static EventType<ConfigurationChanged> ConfigurationChanged = new();

        /// <summary>
        /// Event that triggers after the creation of a configuration
        /// </summary>
        [Obsolete("Use CEvents.ConfigEvents.CreateConfig")]
        public static EventType<ConfigurationCreated> ConfigurationCreated = new();

        /// <summary>
        /// Event that triggers whenever the plugin Starts to create instances of objects and structs.
        /// <br></br>
        /// <br></br>
        /// Called once.
        /// </summary>
        [Obsolete("Use CEvents.LifecycleEvents.PluginStart")]
        public static EventType<EventArgs> PluginInitialized = new();

        /// <summary>
        /// Event that triggers when the plugin starts to patch methods and classes.
        /// <br></br>
        /// <br></br>
        /// Called once.
        /// </summary>
        [Obsolete("Use CEvents.LifecycleEvents.PluginStart")]
        public static EventType<EventArgs> PluginSetup = new();

        /// <summary>
        /// Event that triggers when the plugin is fully built.
        /// <br></br>
        /// <br></br>
        /// Called once.
        /// </summary>
        [Obsolete("Use CEvents.LifecycleEvents.PluginStart")]
        public static EventType<EventArgs> PluginEnabled = new();

        [Obsolete("Use CEvents.MenuEvents.Create")]
        public static EventType<EventArgs> BeforeMenuDisplay = new();

        [Obsolete("Use CEvents.MenuEvents.Prepare")]
        public static EventType<EventArgs> AfterMenuDisplay = new();
    }
}
