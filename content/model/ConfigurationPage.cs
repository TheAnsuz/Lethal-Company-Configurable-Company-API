using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class ConfigurationPage
    {
        private static readonly List<ConfigurationPage> _pages = new();

        private static ConfigurationPage _default;
        public static ConfigurationPage Default
        {
            get
            {
                _default ??= LethalConfiguration.CreatePage()
                    .SetName("Default page")
                    .Build();

                return _default;
            }
        }

        public static IReadOnlyList<ConfigurationPage> GetAll() => _pages;

        public readonly string Name;
        public readonly int Number;

        public ConfigurationPage(ConfigurationPageBuilder builder)
        {
            if (builder == null)
                throw new ArgumentException($"Tried to create page without ConfigurationPageBuilder");

            if (builder.Name == null)
                throw new ArgumentException($"Tried to create page without a name");

            Name = builder.Name;
            Number = _pages.Count;

            _pages.Insert(Number, this);
        }

        public override string ToString()
        {
            return $"ConfigurationPage({Name}, {Number})";
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * Number;
        }

        public override bool Equals(object obj)
        {
            return obj is ConfigurationPage other ? GetHashCode() == other.GetHashCode() : false;
        }
    }
}
