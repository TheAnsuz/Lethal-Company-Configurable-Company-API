using System;

namespace Amrv.ConfigurableCompany.content.model.events
{
    [Obsolete("Use CEvents.ConfigEvents...")]
    public class ConfigurationCreated : EventArgs
    {
        public readonly Configuration Configuration;

        internal ConfigurationCreated(Configuration Configuration)
        {
            this.Configuration = Configuration;
        }
    }
}
