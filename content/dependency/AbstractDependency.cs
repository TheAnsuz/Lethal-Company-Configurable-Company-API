using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Amrv.ConfigurableCompany.content.dependency
{
    internal abstract class AbstractDependency
    {
        public Assembly Assembly { get; internal set; }

        public bool IsPresent => Assembly != null;

        public abstract string PluginGUID { get; }

        public abstract void Instantiate(Harmony harmony);

        private static readonly List<AbstractDependency> _dependencies = new();

        public static void Register<T>(T dependency) where T : AbstractDependency
        {
            _dependencies.Add(dependency);
        }

        internal static void TryFindAll(Harmony harmony)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (AbstractDependency abstractDependency in _dependencies)
                {
                    if (assembly.GetName().Name.Equals(abstractDependency.PluginGUID))
                    {
                        ConfigurableCompanyPlugin.Info($"Found dependency {abstractDependency.PluginGUID} {assembly.GetName().Version}");
                        abstractDependency.Assembly = assembly;
                        abstractDependency.Instantiate(harmony);
                    }
                }
            }
        }
    }
}
