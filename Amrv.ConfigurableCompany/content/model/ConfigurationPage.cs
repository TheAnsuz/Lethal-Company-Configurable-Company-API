using Amrv.ConfigurableCompany.API;
using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.content.model
{
    [Obsolete("Use CPage")]
    public sealed class ConfigurationPage
    {
        private static readonly List<ConfigurationPage> _pages = new();

        private static ConfigurationPage _default;
        public static ConfigurationPage Default
        {
            get
            {
                _default ??= new ConfigurationPageBuilder()
                    .SetName("Default page")
                    .Build();

                return _default;
            }
        }

        public static IReadOnlyList<ConfigurationPage> GetAll() => _pages;

        internal readonly CPage Page;

        public string Name => Page.Name;
        public readonly int Number = 0;

        public ConfigurationPage(ConfigurationPageBuilder builder)
        {
            if (builder == null)
                throw new ArgumentException($"Tried to create page without ConfigurationPageBuilder");

            Page = builder.CBuilder;

            _pages.Insert(Number, this);
        }

        public override string ToString()
        {
            return $"ConfigurationPage({Name}, {Number})";
        }

        public override int GetHashCode() => Page.GetHashCode() - 10;

        public override bool Equals(object obj)
        {
            return obj is ConfigurationPage other && GetHashCode() == other.GetHashCode();
        }
    }
}
