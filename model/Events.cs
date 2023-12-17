using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model
{
    public class Events
    {
        public delegate bool OnConfigurationChange(ChangeReason reason, object oldValue, ref object newValue);
    }
}
