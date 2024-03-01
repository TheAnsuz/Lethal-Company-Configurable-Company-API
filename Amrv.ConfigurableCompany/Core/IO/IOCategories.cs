using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils.IO;
using System.IO;

namespace Amrv.ConfigurableCompany.Core.IO
{
    internal static class IOCategories
    {
        private static readonly CCATFile File = new(Path.Combine(ConfigurableCompanyPlugin.DataFolder, "categories.ccat"));

        public static void Load()
        {
            File.Read();
        }

        public static void Save()
        {
            File.Write();
        }

        public static bool GetOpenState(CCategory category)
        {
            if (File.TryGetState(category.ID, out var state))
            {
                return state;
            }
            return false;
        }

        public static void SetOpenState(CCategory category, bool open)
        {
            File.SetState(category.ID, open);
        }
    }
}
