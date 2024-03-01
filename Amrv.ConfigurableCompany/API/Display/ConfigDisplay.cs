using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API.Display
{
    public abstract class ConfigDisplay
    {
        private GameObject _container;
        /// <summary>
        /// The GameObject that acts as a panel and container of the display entry
        /// </summary>
        public GameObject Container
        {
            get
            {
                _container ??= CreateContainer(Config);
                return _container;
            }
        }

        /// <summary>
        /// The configuration that is being displayed by this instance
        /// </summary>
        public CConfig Config { get; private set; }

        internal GameObject Create(CConfig config)
        {
            Config = config;
            GameObject container = Container;
            LoadFromConfig(config.Value);
            WhenToggled(config.Enabled);
            return container;
        }

        /// <summary>
        /// This method should return the GameObject holding the display of the configuration and should adjust how it's shown using the information of the configuration.
        /// </summary>
        /// <param name="config">The configuration set for this instance</param>
        /// <returns>The GameObject containing the panel</returns>
        protected abstract GameObject CreateContainer(CConfig config);

        /// <summary>
        /// This method should adjust the values and information shown in the display from the value that the configuration has.
        /// </summary>
        /// <param name="value">The raw value that the configuration has</param>
        protected internal abstract void LoadFromConfig(in object value);

        /// <summary>
        /// This method should return the converted or convertable value that is being displayed to be stored in the configuration.
        /// </summary>
        /// <param name="value">The value that will be set in the configuration</param>
        protected internal abstract void SaveToConfig(out object value);

        // Functions

        /// <summary>
        /// A trigger that will be called when the user requests to reset the configuration.
        /// </summary>
        protected internal virtual void WhenReset() { }

        /// <summary>
        /// A trigger that will be called when the user requests to restore the configuration.
        /// </summary>
        protected internal virtual void WhenRestored() { }

        /// <summary>
        /// A trigget that will be called when the user requests to change the status of the configuration.
        /// </summary>
        /// <param name="enabled">True if the configuration is now enabled, false otherwise</param>
        protected internal abstract void WhenToggled(bool enabled);

        internal Action _resetCallback;

        /// <summary>
        /// Call an internal reset of this configuration.
        /// <para></para>
        /// This method will propagate the event for the whole system and cause all the events to be triggered.
        /// </summary>
        protected internal void Reset()
        {
            _resetCallback?.Invoke();
        }

        internal Action _restoreCallback;

        /// <summary>
        /// Call an internal restore of this configuration.
        /// <para></para>
        /// This method will propagate the event for the whole system and cause all the events to be triggered.
        /// </summary>
        protected internal void Restore()
        {
            _restoreCallback?.Invoke();
        }

        internal Action<bool> _toggleCallback;

        /// <summary>
        /// Call an internal toggle for the binded configuration
        /// <para></para>
        /// This method will propagate the event for the whole system and cause all the events to be triggered.
        /// </summary>
        protected void Toggle() => Toggle(!Config.Enabled);

        /// <summary>
        /// Call an internal toggle for the binded configuration
        /// <para></para>
        /// This method will propagate the event for the whole system and cause all the events to be triggered.
        /// </summary>
        /// <param name="enabled">The new status to be set</param>
        protected internal void Toggle(bool enabled)
        {
            _toggleCallback?.Invoke(enabled);
        }
    }
}
