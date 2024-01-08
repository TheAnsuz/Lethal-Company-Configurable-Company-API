using Amrv.ConfigurableCompany.content.patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrv.ConfigurableCompany.content.model
{
    public class ConfigurationPageBuilder
    {
        public bool Editable { get; private set; } = true;

        private string _name;
        public string Name { get => _name; set { if (Editable) _name = value; } }

        public ConfigurationPageBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public ConfigurationPage Build()
        {
            Editable = false;
            ConfigurationPage page = new(this);

            return page;
        }

        public static implicit operator ConfigurationPage(ConfigurationPageBuilder builder)
        {
            return builder.Build();
        }
    }
}
