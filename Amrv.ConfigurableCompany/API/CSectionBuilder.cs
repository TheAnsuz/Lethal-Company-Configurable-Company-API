using Amrv.ConfigurableCompany.API.Accesors;
using Amrv.ConfigurableCompany.Core;
using System;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CSectionBuilder : InstanceBuilder<CSection>
    {
        public string ID;
        public string Name;
        [Obsolete("Use BCategory")]
        public string Category;

        public BuildCategory BCategory { get; set; }

        [Obsolete("Use BCategory")]
        public CCategory CCategory
        {
            set => BCategory = value;
            get => BCategory;
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
            BCategory = categoryId;
            return this;
        }

        public CSectionBuilder SetCategory(CCategory category)
        {
            BCategory = category.ID;
            return this;
        }

        protected override CSection BuildInstance()
        {
            if (Category != null)
                BCategory ??= Category;

            BCategory ??= CCategory.Default;

            return new CSection(this);
        }

        protected override bool TryGetExisting(out CSection item)
        {
            return CSection.Storage.TryGetValue(ID, out item);
        }
    }
}
