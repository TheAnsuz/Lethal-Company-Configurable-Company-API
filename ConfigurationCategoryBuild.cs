using ConfigurableCompany.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany
{
    public sealed class ConfigurationCategoryBuild
    {
        private readonly string _id;
        private string _displayName;

        internal ConfigurationCategoryBuild(string id)
        {
            _id = id;
        }

        public ConfigurationCategoryBuild SetName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        public ConfigurationCategory Build() => ConfigurationCategory.Create(_id, _displayName);
    }
}
