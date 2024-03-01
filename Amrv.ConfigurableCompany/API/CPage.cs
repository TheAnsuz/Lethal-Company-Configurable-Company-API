using Amrv.ConfigurableCompany.Core;
using Amrv.ConfigurableCompany.Core.Config;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CPage
    {
        private static CPage _defaultPage;

        public static CPage Default
        {
            get
            {
                _defaultPage ??= new CPageBuilder()
                {
                    ID = "configurable-company_page_default",
                    Name = "Default",
                    Description = "Configuration page provided by CCompany"
                };

                return _defaultPage;
            }
        }

        private static readonly Dictionary<string, CPage> _pages = [];
        public static readonly IDStorage<CPage> Storage = new(_pages);

        public static CPageBuilder Builder() => new();

        private readonly Dictionary<string, CCategory> _categories;
        public readonly IDStorage<CCategory> Categories;

        internal void AddCategory(CCategory category) => _categories.Add(category.ID, category);
        internal void RemoveCategory(CCategory category) => _categories.Remove(category.ID);

        public readonly string ID;
        public readonly string Name;
        public readonly string Description;

        internal CPage(CPageBuilder builder)
        {
            // Checks
            if (builder == null) throw new BuildingException("Tried to create page without builder");

            if (builder.ID == null) throw new BuildingException("Unable to build page without ID");

            if (_pages.ContainsKey(builder.ID)) throw new BuildingException($"Tried to create a page with an existing ID ({builder.ID})");

            // Sets
            ID = builder.ID;
            Name = builder.Name ?? "";
            Description = builder.Description ?? "";

            // Indexing
            _categories = [];
            Categories = new(_categories);

            // Index itself
            _pages.Add(ID, this);

            // Notify
            ConfigEventRouter.OnCreate_Page(this);
        }

        public override bool Equals(object obj)
        {
            return obj is CPage other && other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            return 1507863948 | ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"Page[{ID}:{Name}]";
        }
    }
}
