using Amrv.ConfigurableCompany.content.model.events;
using System;
namespace Amrv.ConfigurableCompany
{
    public class Events
    {
        private Events() { }

        /// <summary>
        /// Event that triggers every time a configuration value has changed.
        /// </summary>
        public static EventType<ConfigurationChanged> ConfigurationChanged = new();

        /// <summary>
        /// Event that triggers after the creation of a configuration
        /// </summary>
        public static EventType<ConfigurationCreated> ConfigurationCreated = new();

        /// <summary>
        /// Event that triggers whenever the plugin Starts to create instances of objects and structs.
        /// <br></br>
        /// <br></br>
        /// Called once.
        /// </summary>
        public static EventType<EventArgs> PluginInitialized = new();

        /// <summary>
        /// Event that triggers when the plugin starts to patch methods and classes.
        /// <br></br>
        /// <br></br>
        /// Called once.
        /// </summary>
        public static EventType<EventArgs> PluginSetup = new();

        /// <summary>
        /// Event that triggers when the plugin is fully built.
        /// <br></br>
        /// <br></br>
        /// Called once.
        /// </summary>
        public static EventType<EventArgs> PluginEnabled = new();

        public static EventType<EventArgs> BeforeMenuDisplay = new();
        public static EventType<EventArgs> AfterMenuDisplay = new();
    }
}
