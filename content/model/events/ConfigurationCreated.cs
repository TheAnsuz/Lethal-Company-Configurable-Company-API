using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
