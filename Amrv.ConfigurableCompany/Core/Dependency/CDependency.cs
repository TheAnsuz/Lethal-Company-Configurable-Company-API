using HarmonyLib;
using System.Reflection;

namespace Amrv.ConfigurableCompany.Core.Dependency
{
    public abstract class CDependency
    {
        public Assembly Assembly { get; internal set; }

        public abstract string AssemblyName { get; }

        public abstract void Process(Harmony harmony);
    }
}
