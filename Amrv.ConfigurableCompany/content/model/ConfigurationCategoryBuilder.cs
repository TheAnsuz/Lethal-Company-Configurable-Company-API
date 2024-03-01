using Amrv.ConfigurableCompany.API;
using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.model
{
    [Obsolete("Use CCategoryBuilder")]
    public class ConfigurationCategoryBuilder
    {
        internal readonly CCategoryBuilder CBuilder = new();

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

        public bool HidesIfEmpty
        {
            get => CBuilder.HideIfEmpty;
            set => CBuilder.HideIfEmpty = value;
        }

        public Color Color
        {
            get => CBuilder.Color;
            set => CBuilder.Color = value;
        }

        private ConfigurationPage _page = null;
        public ConfigurationPage Page
        {
            get => _page;
            set
            {
                _page = value;
                CBuilder.Page = value.Page.ID;
            }
        }

        protected internal ConfigurationCategoryBuilder() { }
        protected internal ConfigurationCategoryBuilder(string id)
        {
            ID = id;
        }

        public ConfigurationCategoryBuilder SetID(string id)
        {
            ID = id;
            return this;
        }

        public ConfigurationCategoryBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public ConfigurationCategoryBuilder SetColor(Color color)
        {
            Color = color;
            return this;
        }

        public ConfigurationCategoryBuilder SetColorRGB(byte red, byte green, byte blue, byte alpha = 255)
        {
            Color = new Color32(red, green, blue, alpha);
            return this;
        }

        public ConfigurationCategoryBuilder SetColor32(Color32 color)
        {
            Color = color;
            return this;
        }

        public ConfigurationCategoryBuilder HideIfEmpty(bool hide = true)
        {
            HidesIfEmpty = hide;
            return this;
        }

        public ConfigurationCategoryBuilder SetPage(ConfigurationPage page)
        {
            Page = page;
            return this;
        }

        public ConfigurationCategory Build()
        {
            Page ??= ConfigurationPage.Default;

            ConfigurationCategory category = new(this);

            return category;
        }

        public static implicit operator ConfigurationCategory(ConfigurationCategoryBuilder builder)
        {
            return builder.Build();
        }
    }
}
