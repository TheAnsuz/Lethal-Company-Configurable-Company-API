using Amrv.ConfigurableCompany.Plugin;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Amrv.ConfigurableCompany.Core.Dependency
{
    public static class DependencyManager
    {
        private static readonly List<CDependency> _dependencies = [];

        static DependencyManager()
        {
            _dependencies.Add(new BetterSavesDependency());
        }

        public static void CheckDependencies(Harmony harmony)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                //ConfigurableCompanyPlugin.Info($"Detected loaded assembly: {assembly.FullName}");
                foreach (CDependency dependency in _dependencies)
                {
                    //ConfigurableCompanyPlugin.Info($">>> Checking {dependency.AssemblyName}");

                    if (assembly.GetName().Name.Equals(dependency.AssemblyName))
                    {
                        ConfigurableCompanyPlugin.Info($"Found dependency {dependency.AssemblyName} [{assembly.GetName().Version}]");
                        dependency.Assembly = assembly;
                        dependency.Process(harmony);
                    }
                }
            }
        }
    }
}
