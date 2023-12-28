using System;

namespace Amrv.ConfigurableCompany.content.model.events
{
    public class ConfigurationCreated : EventArgs
    {
        public readonly Configuration Configuration;

        internal ConfigurationCreated(Configuration Configuration)
        {
            this.Configuration = Configuration;
        }
    }
}
