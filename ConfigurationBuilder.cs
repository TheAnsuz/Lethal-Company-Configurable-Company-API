using ConfigurableCompany.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany
{
    public sealed class ConfigurationBuilder
    {
        private ConfigurationBuilder() { }

        public static ConfigurationBuild NewConfig(string id) => new(id);

        public static ConfigurationCategoryBuild NewCategory(string id) => new(id);
    }
}
