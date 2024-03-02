using Amrv.ConfigurableCompany.Core;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CPageBuilder : InstanceBuilder<CPage>
    {
        public string ID;
        public string Name;
        public string Description;

        public CPageBuilder SetID(string id)
        {
            ID = id;
            return this;
        }

        public CPageBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public CPageBuilder SetDescription(string description)
        {
            Description = description;
            return this;
        }

        protected override CPage BuildInstance()
        {
            return new CPage(this);
        }

        protected override bool TryGetExisting(out CPage item)
        {
            return CPage.Storage.TryGetValue(ID, out item);
        }
    }
}
