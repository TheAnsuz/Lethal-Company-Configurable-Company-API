using Amrv.ConfigurableCompany.content.display;
using System;

namespace Amrv.ConfigurableCompany.content.model
{
    public abstract class ConfigurationType
    {
        public abstract string Name { get; }

        /// <summary>
        /// Starts the process of constructing a new configuration of this type
        /// <br></br>
        /// <br></br>
        /// Note that exceptions may occur if the provided values in the ConfigurationBuilder are not valid.
        /// <br></br>
        /// It's up to the implementation to determine which ones are valid and which ones aren't.
        /// </summary>
        /// <param name="builder">The ConfigurationBuilder containing all the information required for this Configuration to be created</param>
        /// <returns>A new Configuration instance</returns>
        public Configuration CreateConfig(ConfigurationBuilder builder)
        {
            // Starts the creation of a configuration, all the internal checks are done here.
            // External checks are done after this method within the CreateNewConfiguration
            // The configuration constructor will do the necesary checks for the configs to work, but only the required for nulls

            Configuration config = CreateNewConfiguration(builder);
            if (IngameMenu.ConfigDisplay?.ConfigurationMenu.Pages[config.Category.Page.Number].TryGet(config.Category.ID, out ConfigurationCategoryDisplay display) ?? false)
            {
                display.Add(config.Type.CreateConfigurationDisplay(config));
            }
            Events.ConfigurationCreated.Invoke(new(config));
            return config;
        }

        /// <summary>
        /// Starts the process of constructing a new configuration display of this type.
        /// <br></br>
        /// If the display already exists then its returned
        /// </summary>
        /// <param name="config">The configuration that holds the information for this display</param>
        /// <returns>A new ConfigurationDisplay or the already existing one</returns>
        public ConfigurationItemDisplay CreateDisplay(Configuration config)
        {
            // If already exists return that
            ConfigurationItemDisplay display = CreateConfigurationDisplay(config);
            return display;
        }

        /// <summary>
        /// Gets the default value that this configuration may contain when there is not valid value that can be set
        /// </summary>
        /// <returns>The default value of the config</returns>
        public abstract object DefaultValue { get; }

        /// <summary>
        /// Checks whenever a value is valid matches the requirements to be set for any configuration that uses this type
        /// </summary>
        /// <param name="value">The new value that needs to be check</param>
        /// <returns>True if the value can be directly set, False if not</returns>
        public abstract bool IsValidValue(object value);

        /// <summary>
        /// Tries to convert a object to a value that this ConfigurationType accepts.
        /// <br></br>
        /// <br></br>
        /// Internally, this method will be called if the method <code>IsValidValue</code> returns False whenever a new value tries to be set.
        /// </summary>
        /// <param name="value">The object to convert</param>
        /// <param name="result">The resulting object converted or nothing if the conversion failed</param>
        /// <returns>True if the conversion succeded, False otherwise</returns>
        public abstract bool TryConvert(object value, out object result);

        public bool TryGetAs<T>(object value, out T result) => TryGetAs<T>(value, out result, typeof(T), Type.GetTypeCode(typeof(T)));

        public abstract bool TryGetAs<T>(object value, out T result, Type type, TypeCode code);

        /// <summary>
        /// This method creates the new configuration. The implementation could create additional checks and do additional tasks upon completion
        /// </summary>
        /// <param name="builder">The ConfigurationBuilder containing all the information</param>
        /// <returns>The newly created Configuration</returns>
        protected virtual Configuration CreateNewConfiguration(ConfigurationBuilder builder)
        {
            return new Configuration(builder);
        }

        /// <summary>
        /// This method creates the display for a configuration. The implementation could modify the object to be created to its own.
        /// <br></br>
        /// <br></br>
        /// Each configuration will only have one display and thus. only one display can exist for each configuration.
        /// </summary>
        /// <param name="config">An already existing configuration</param>
        /// <returns>The display for the config</returns>
        protected abstract ConfigurationItemDisplay CreateConfigurationDisplay(Configuration config);
    }
}
