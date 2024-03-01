using Amrv.ConfigurableCompany.Core;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CSectionBuilder : InstanceBuilder<CSection>
    {
        public string ID;
        public string Name;
        public string Category;

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

        public CSectionBuilder SetID(string id)
        {
            ID = id;
            return this;
        }

        public CSectionBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public CSectionBuilder SetCategory(string categoryId)
        {
            Category = categoryId;
            return this;
        }

        public CSectionBuilder SetCategory(CCategory category)
        {
            Category = category.ID;
            return this;
        }

        protected override CSection BuildInstance()
        {
            CCategory ??= CCategory.Default;

            return new CSection(this);
        }

        protected override bool TryGetExisting(out CSection item)
        {
            return CSection.Storage.TryGetValue(ID, out item);
        }
    }
}
