using Amrv.ConfigurableCompany.API;
using System;

namespace Amrv.ConfigurableCompany.content.model
{
    [Obsolete("Use CConfigBuilder")]
    public class ConfigurationBuilder
    {
        internal readonly CConfigBuilder CBuilder = new();

        public string ID
        {
            get => CBuilder.ID;
            set => CBuilder.ID = value;
        }
        public string Name
        {
            get => CBuilder.Name;
            set => CBuilder.Name = value;
        }

        public object Value
        {
            get => CBuilder.Value;
            set => CBuilder.Value = value;
        }

        public bool Synchronized
        {
            get => CBuilder.Synchronized;
            set => CBuilder.Synchronized = value;
        }

        public bool Experimental
        {
            get => CBuilder.Experimental;
            set => CBuilder.Experimental = value;
        }


        [Obsolete("Pleasle, try to implement your settings in a way users won't need to restart the game\nIf you need help with that, you can check out Lethal Company Variables does it or ask for help")]
        public bool NeedsRestart
        {
            get;
            set;
        }

        public string Tooltip
        {
            get => CBuilder.Tooltip;
            set => CBuilder.Tooltip = value;
        }

        private ConfigurationCategory _category;
        public ConfigurationCategory Category
        {
            get => _category;
            set
            {
                if (value == null)
                    _category = ConfigurationCategory.Default;

                CBuilder.Category = value.CCategory.ID;
                _category = value;
            }
        }

        /// <summary>
        /// The category name of the category in where this configuration will be shown. All configurations must be inside a category and if no one is provided a default one will be used
        /// </summary>
        public string CategoryName
        {
            get => _category.Name;
            set
            {
                if (value == null)
                    _category = ConfigurationCategory.Default;

                CBuilder.Category = value;

                ConfigurationCategory.TryGet(value, out _category);
            }
        }

        private ConfigurationType _type;
        /// <summary>
        /// The type of value that this configuration accepts. You can create your own type or use the existing ones in ConfigurationTypes
        /// </summary>
        public ConfigurationType Type
        {
            get => _type;
            set
            {
                if (value != null)
                    _type = value;

                CBuilder.Type = value.CType;
            }
        }

        protected internal ConfigurationBuilder(string id)
        {
            ID = id;
        }

        protected internal ConfigurationBuilder() { }

        public ConfigurationBuilder SetID(string id)
        {
            ID = id;
            return this;
        }

        public ConfigurationBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public ConfigurationBuilder SetTooltip(params string[] lines)
        {
            Tooltip = string.Join("\n", lines);
            return this;
        }

        public ConfigurationBuilder SetTooltip(string tooltip)
        {
            Tooltip = tooltip;
            return this;
        }

        public ConfigurationBuilder SetValue(object value)
        {
            Value = value;
            return this;
        }

        public ConfigurationBuilder SetSynchronized(bool sync)
        {
            Synchronized = sync;
            return this;
        }

        public ConfigurationBuilder SetExperimental(bool experimental)
        {
            Experimental = experimental;
            return this;
        }

        public ConfigurationBuilder SetNeedsRestart(bool needsRestart)
        {
            NeedsRestart = needsRestart;
            return this;
        }

        public ConfigurationBuilder SetCategory(ConfigurationCategory category)
        {
            Category = category;
            return this;
        }

        public ConfigurationBuilder SetCategory(string categoryName)
        {
            CategoryName = categoryName;
            return this;
        }

        public ConfigurationBuilder SetType(ConfigurationType type)
        {
            Type = type;
            return this;
        }

        public Configuration Build()
        {
            Category ??= ConfigurationCategory.Default;
            return Type.CreateConfig(this);
        }

        public static implicit operator Configuration(ConfigurationBuilder builder)
        {
            return builder.Build();
        }
    }
}
