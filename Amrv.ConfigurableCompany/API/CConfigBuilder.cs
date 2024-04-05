using Amrv.ConfigurableCompany.API.Accesors;
using Amrv.ConfigurableCompany.Core;
using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CConfigBuilder : InstanceBuilder<CConfig>
    {
        public string ID;
        public string Name;
        [Obsolete("Use BSection")]
        public string Section;
        [Obsolete("Use BCategory")]
        public string Category;
        [Obsolete("Use BTooltip")]
        public string Tooltip;
        public CType Type;
        public object DefaultValue;
        public object Value;
        public bool Enabled;
        public bool Experimental;
        public bool Synchronized;
        public bool Toggleable;

        public BuildSection BSection { get; set; }
        public BuildTooltip BTooltip { get; set; }
        public BuildCategory BCategory { get; set; }

        [Obsolete("Use BCategory")]
        public CCategory CCategory
        {
            set => BCategory = value;
            get => BCategory;
        }

        [Obsolete("Use BSection")]
        public CSection CSection
        {
            set => BSection = value;
            get => BSection;
        }

        [Obsolete("Use BTooltip")]
        public string[] Tooltips
        {
            get => BTooltip;
            set => BTooltip = value;
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
            BSection = sectionId;
            return this;
        }

        public CConfigBuilder SetSection(CSection section)
        {
            BSection = section.ID;
            return this;
        }

        public CConfigBuilder SetCategory(string categoryId)
        {
            BCategory = categoryId;
            return this;
        }

        public CConfigBuilder SetCategory(CCategory category)
        {
            BCategory = category.ID;
            return this;
        }

        public CConfigBuilder SetToolip(string tooltip)
        {
            BTooltip = tooltip;
            return this;
        }

        public CConfigBuilder SetTooltip(params string[] lines)
        {
            BTooltip = string.Join("\n", lines);
            return this;
        }

        public CConfigBuilder SetTooltip(IEnumerable<string> lines)
        {
            BTooltip = string.Join("\n", lines);
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
            Toggleable = toggleable;
            return this;
        }

        protected override CConfig BuildInstance()
        {
            if (Section != null)
                BSection ??= Section;

            if (Category != null)
                BCategory ??= Category;

            if (Tooltip != null)
                BTooltip ??= Tooltip;

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
