using Amrv.ConfigurableCompany.content.model.data;
using System;

namespace Amrv.ConfigurableCompany.content.model.events
{
    [Obsolete("Use CEvents.ConfigEvents...")]
    public class ConfigurationChanged : EventArgs
    {
        /// <summary>
        /// The configuration that suffered the change
        /// </summary>
        public readonly Configuration Configuration;
        /// <summary>
        /// The previous value that the configuration held
        /// </summary>
        public readonly object OldValue;
        /// <summary>
        /// The new and current value the configuration has (Configurations that require a restart will continue holding the previous value)
        /// </summary>
        public readonly object NewValue;
        /// <summary>
        /// The reason this change occurred
        /// </summary>
        public readonly ChangeReason Reason;
        /// <summary>
        /// The result of the change. Setting a value may cause a configuration conversion or fail if the value is not valid
        /// </summary>
        public readonly ChangeResult Result;

        public ConfigurationChanged(Configuration configuration, object oldValue, object newValue, ChangeReason reason, ChangeResult result)
        {
            Configuration = configuration;
            OldValue = oldValue;
            NewValue = newValue;
            Reason = reason;
            Result = result;
        }
    }
}
