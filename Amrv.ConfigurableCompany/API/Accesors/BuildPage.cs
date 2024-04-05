using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.API.Accesors
{
    public sealed class BuildPage
    {
        private readonly string _id;

        private BuildPage(string id)
        {
            _id = id;
        }

        public static implicit operator BuildPage(string name)
        {
            return new(name);
        }

        public static implicit operator BuildPage(CPage category)
        {
            return new(category.ID);
        }

        public static implicit operator string(BuildPage category)
        {
            return category._id;
        }

        public static implicit operator CPage(BuildPage category)
        {
            if (CPage.Storage.TryGetValue(category._id, out var built))
            {
                return built;
            }
            return null;
        }
    }
}
