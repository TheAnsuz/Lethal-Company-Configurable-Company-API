using Amrv.ConfigurableCompany.content.patch;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.model
{
    public class ConfigurationCategoryBuilder
    {
        public bool Editable { get; private set; } = true;

        private string _id;
        public string ID { get => _id; set { if (Editable) _id = value; } }

        private string _name;
        public string Name { get => _name; set { if (Editable) _name = value; } }

        private bool _hidesIfEmpty = false;
        public bool HidesIfEmpty { get => _hidesIfEmpty; set { if (Editable) _hidesIfEmpty = value; } }

        private Color _color = Color.white;
        public Color Color { get => _color; set { if (Editable) _color = value; } }

        internal protected ConfigurationCategoryBuilder() { }
        internal protected ConfigurationCategoryBuilder(string id)
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

        public ConfigurationCategory Build()
        {
            Editable = false;
            ConfigurationCategory category = new(this);
            DisplayConfigurationPatch.ConfigDisplay?.AddCategory(category);
            return category;
        }

        public static implicit operator ConfigurationCategory(ConfigurationCategoryBuilder builder)
        {
            return builder.Build();
        }
    }
}
