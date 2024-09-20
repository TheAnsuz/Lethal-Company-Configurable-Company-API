using Amrv.ConfigurableCompany.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Core
{
    internal static class CCache
    {
        private static string _userSeed;
        public static string UsedSeed
        {
            get => _userSeed;
            set
            {
                ConfigurableCompanyPlugin.Debug($"Set cache UserSeed to {value}");
                _userSeed = value;
            }
        }
    }
}
