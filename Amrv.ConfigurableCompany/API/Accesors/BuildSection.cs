using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.API.Accesors
{
    public sealed class BuildSection
    {
        private readonly string _id;

        private BuildSection(string id)
        {
            _id = id;
        }

        public static implicit operator BuildSection(string name)
        {
            return new(name);
        }

        public static implicit operator BuildSection(CSection section)
        {
            return new(section.ID);
        }

        public static implicit operator string(BuildSection section)
        {
            return section._id;
        }

        public static implicit operator CSection(BuildSection section)
        {
            if (CSection.Storage.TryGetValue(section._id, out var built))
            {
                return built;
            }
            return null;
        }
    }
}
