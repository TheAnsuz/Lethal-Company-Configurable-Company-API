using System.Collections.Generic;
using System.Reflection;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class AssemblyRegistry
    {
        private AssemblyRegistry() { }

        private static readonly Dictionary<string, Assembly> _registers = new();

        public static void RegisterSelf()
        {
            Assembly calling = Assembly.GetCallingAssembly();
            _registers[calling.GetName().Name] = calling;
        }

        internal static void RegisterAssembly(Assembly assembly)
        {
            _registers[assembly.GetName().Name] = assembly;
        }
    }
}
