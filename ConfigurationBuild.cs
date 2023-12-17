using ConfigurableCompany.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany
{
    public sealed class ConfigurationBuild
    {
        private readonly string _id;
        private string _name;
        private ConfigurationCategory _category;
        private ConfigurationType _type;
        private object _value;
        private string _tooltip;
        private bool _syncronized;
        private Events.OnConfigurationChange _onConfigurationChange;

        public string Name { set => SetName(value); }
        public string CategoryName { set => SetCategory(value); }
        public ConfigurationCategory Category { set => SetCategory(value); }
        public ConfigurationType Type { set => SetType(value); }
        public object Value { set => SetValue(value); }
        public string Tooltip { set => SetTooltip(value); }
        public bool Syncronized { set => SetSyncronized(value); }
        public Events.OnConfigurationChange OnConfigruationChange { set => Event_OnConfigurationSet(value); }

        internal ConfigurationBuild(string id)
        {
            _id = id;
            _name = id;
            _type = ConfigurationType.String;
            _category = ConfigurationCategory.DEFAULT;
            _value = "null";
            _tooltip = default;
            _syncronized = false;
        }

        public ConfigurationBuild SetName(string name)
        {
            _name = name;
            return this;
        }

        public ConfigurationBuild SetType(ConfigurationType type)
        {
            _type = type;
            return this;
        }

        public ConfigurationBuild SetValue(object value)
        {
            _value = value;
            return this;
        }

        public ConfigurationBuild SetCategory(ConfigurationCategory category)
        {
            if (category == null)
                return this;

            _category = category;
            return this;
        }

        public ConfigurationBuild SetCategory(string categoryID)
        {
            ConfigurationCategory.TryGet(categoryID, out _category);
            return this;
        }

        public ConfigurationBuild SetTooltip(string tooltip)
        {
            _tooltip = tooltip;
            return this;
        }

        public ConfigurationBuild SetTooltip(params string[] lines)
        {
            _tooltip = lines[0] + "\n";
            for (int i = 1; i < lines.Length - 1; i++)
                _tooltip += lines[i] + "\n";

            if (lines.Length > 0)
                _tooltip += lines[lines.Length - 1];

            return this;
        }

        public ConfigurationBuild SetSyncronized(bool syncronized)
        {
            _syncronized = syncronized;
            return this;
        }

        public ConfigurationBuild Event_OnConfigurationSet(Events.OnConfigurationChange onConfigurationChange)
        {
            _onConfigurationChange = onConfigurationChange;
            return this;
        }

        public Configuration Build() => Configuration.Create(_id, _name, _category, _type, _value, _tooltip, _syncronized, _onConfigurationChange);
    }
}
