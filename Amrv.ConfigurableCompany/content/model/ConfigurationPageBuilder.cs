using Amrv.ConfigurableCompany.API;
using System;

namespace Amrv.ConfigurableCompany.content.model
{
    [Obsolete("Use CPageBuilder")]
    public class ConfigurationPageBuilder
    {
        internal readonly CPageBuilder CBuilder = new()
        {
            Description = "Tell the developer to update his mod!"
        };

        public string Name
        {
            get => CBuilder.Name;
            set
            {
                CBuilder.Name = value;
                CBuilder.ID = "_" + value.ToLower().Replace(" ", "-") + "_";
            }
        }

        public ConfigurationPageBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public ConfigurationPage Build()
        {
            ConfigurationPage page = new(this);

            return page;
        }

        public static implicit operator ConfigurationPage(ConfigurationPageBuilder builder)
        {
            return builder.Build();
        }
    }
}
