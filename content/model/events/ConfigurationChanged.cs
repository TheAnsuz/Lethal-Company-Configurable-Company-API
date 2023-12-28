using Amrv.ConfigurableCompany.content.model.data;
using System;

namespace Amrv.ConfigurableCompany.content.model.events
{
    public class ConfigurationChanged : EventArgs
    {
        public readonly Configuration Configuration;
        public readonly object OldValue;
        public readonly object NewValue;
        public readonly ChangeReason Reason;
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
