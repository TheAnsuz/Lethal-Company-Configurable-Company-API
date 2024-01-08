using Amrv.ConfigurableCompany.content.utils;

namespace Amrv.ConfigurableCompany.content.model
{
    public class ConfigurationBuilder
    {
        /// <summary>
        /// If this builder can be modified or not.
        /// Builders are editable until the Build process starts
        /// </summary>
        public bool Editable { get; private set; } = true;

        private string _id;
        /// <summary>
        /// The string id for the configuration.
        /// <br></br>
        /// This id must be lowercase, only contain letters or numbers and must not contain whitespace. It can also contain underscores and hypens
        /// </summary>
        public string ID { get => _id; set { if (Editable) _id = value; } }

        private string _name;
        /// <summary>
        /// The display name that the configuration will have. Keep it short
        /// </summary>
        public string Name { get => _name; set { if (Editable) _name = value; } }

        private object _value;
        /// <summary>
        /// The value that will have the configuration by default. It must be valid according to the provided Type
        /// </summary>
        public object Value { get => _value; set { if (Editable) _value = value; } }

        private bool _synchronized;
        /// <summary>
        /// Whenever this configuration should be synchronized with the clients when they join the game
        /// </summary>
        public bool Synchronized { get => _synchronized; set { if (Editable) _synchronized = value; } }

        private bool _experimental;
        /// <summary>
        /// Mark configurations as experimentals if they are not guaranteed to work or are a work in progress.
        /// </summary>
        public bool Experimental { get => _experimental; set { if (Editable) _experimental = value; } }

        private bool _needsRestart = false;
        /// <summary>
        /// Mark configurations as Need Restart if the value should only change after the game restarts and not instantly
        /// </summary>
        public bool NeedsRestart { get => _needsRestart; set { if (Editable) _needsRestart = value; } }

        private string _tooltip = "No information provided";
        /// <summary>
        /// The information that will be displayed on the screen when this configuration is hovered.
        /// <br></br>
        /// Explain what the configuration does and how does it work for people to understand it correctly
        /// </summary>
        public string Tooltip { get => _tooltip; set { if (Editable) _tooltip = value; } }

        private ConfigurationCategory _category = ConfigurationCategory.Default;
        /// <summary>
        /// The category in where this configuration will be shown. All configurations must be inside a category and if no one is provided a default one will be used
        /// </summary>
        public ConfigurationCategory Category
        {
            get => _category;
            set
            {
                if (!Editable)
                    return;

                if (value == null)
                    _category = ConfigurationCategory.Default;

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
                if (!Editable)
                    return;

                if (value == null)
                    _category = ConfigurationCategory.Default;

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
                if (!Editable)
                    return;

                if (value != null)
                    _type = value;
            }
        }

        internal protected ConfigurationBuilder(string id)
        {
            ID = id;
        }

        internal protected ConfigurationBuilder() { }

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
            Tooltip = string.Join(DisplayUtils.LINE_SEPARATOR + "", lines);
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

        /// <summary>
        /// Starts the process to create a configuration with the provided parameters. Note that an exception will be thrown if the provided parameters are not valid.
        /// </summary>
        /// <returns>The Built Configuration object ready to use</returns>
        public Configuration Build()
        {
            Category ??= LethalConfiguration.CategoryForMyMod();
            Editable = false;
            return Type.CreateConfig(this);
        }

        public static implicit operator Configuration(ConfigurationBuilder builder)
        {
            return builder.Build();
        }
    }
}
