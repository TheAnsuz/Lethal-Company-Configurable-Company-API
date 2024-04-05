using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.API.Accesors
{
    public sealed class BuildCategory
    {
        private readonly string _id;

        private BuildCategory(string id)
        {
            _id = id;
        }

        public static implicit operator BuildCategory(string name)
        {
            return new(name);
        }

        public static implicit operator BuildCategory(CCategory category)
        {
            return new(category.ID);
        }

        public static implicit operator string(BuildCategory category)
        {
            return category._id;
        }

        public static implicit operator CCategory(BuildCategory category)
        {
            if (CCategory.Storage.TryGetValue(category._id, out var built))
            {
                return built;
            }
            return null;
        }

        public override string ToString()
        {
            return $"BuildSection[id: {_id}]";
        }
    }
}
