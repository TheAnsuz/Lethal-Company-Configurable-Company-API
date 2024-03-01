using Amrv.ConfigurableCompany.Core;
using System;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CConfigBuilder : InstanceBuilder<CConfig>
    {
        public string ID;
        public string Name;
        public string Section;
        public string Category;
        public string Tooltip;
        public CType Type;
        public object DefaultValue;
        public object Value;
        public bool Enabled;
        public bool Experimental;
        public bool Synchronized;
        public bool Toogleable;

        public CCategory CCategory
        {
            set
            {
                Category = value?.ID ?? null;
            }
            get
            {
                if (CCategory.Storage.TryGetValue(Category, out var category))
                {
                    return category;
                }
                return null;
            }
        }

        public CSection CSection
        {
            set
            {
                Section = value?.ID ?? null;
            }
            get
            {
                if (CSection.Storage.TryGetValue(Section, out var section))
                {
                    return section;
                }
                return null;
            }
        }

        public string[] Tooltips
        {
            set
            {
                Tooltip = string.Join("\n", value);
            }
            get
            {
                return Tooltip.Split('\n');
            }
        }

        public CConfigBuilder SetID(string id)
        {
            ID = id;
            return this;
        }

        public CConfigBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public CConfigBuilder SetSection(string sectionId)
        {
            Section = sectionId;
            return this;
        }

        public CConfigBuilder SetSection(CSection section)
        {
            Section = section.ID;
            return this;
        }

        public CConfigBuilder SetCategory(string categoryId)
        {
            Category = categoryId;
            return this;
        }

        public CConfigBuilder SetCategory(CCategory category)
        {
            Category = category.ID;
            return this;
        }

        public CConfigBuilder SetToolip(string tooltip)
        {
            Tooltip = tooltip;
            return this;
        }

        public CConfigBuilder SetTooltip(params string[] lines)
        {
            Tooltip = string.Join("\n", lines);
            return this;
        }

        public CConfigBuilder SetType(CType type)
        {
            Type = type;
            return this;
        }

        public CConfigBuilder SetDefaultValue(object value)
        {
            DefaultValue = value;
            return this;
        }

        public CConfigBuilder SetValue(object value)
        {
            Value = value;
            return this;
        }

        public CConfigBuilder SetEnabled(bool enabled)
        {
            Enabled = enabled;
            return this;
        }

        public CConfigBuilder SetExperimental(bool experimental)
        {
            Experimental = experimental;
            return this;
        }

        public CConfigBuilder SetSynchronized(bool synchronized)
        {
            Synchronized = synchronized;
            return this;
        }

        public CConfigBuilder SetToggleable(bool toggleable)
        {
            Toogleable = toggleable;
            return this;
        }

        protected override CConfig BuildInstance()
        {
            if (Type == null)
            {
                CType.TryGetMapping(Value.GetType(), out Type);
            }

            return new CConfig(this);
        }

        protected override bool TryGetExisting(out CConfig item)
        {
            return CConfig.Storage.TryGetValue(ID, out item);
        }
    }
}
