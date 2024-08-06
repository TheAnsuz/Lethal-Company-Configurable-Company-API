using Amrv.ConfigurableCompany.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.API.Data
{
    public class SpecialSeed
    {
        private static readonly Dictionary<string, SpecialSeed> _specialSeed = [];
        public static readonly IReadOnlyDictionary<string, SpecialSeed> Seeds = new ReadonlyDictionary<string, SpecialSeed>(_specialSeed);

        public static bool IsSpecialSeed(string seed, out SpecialSeed specialSeed) => Seeds.TryGetValue(seed, out specialSeed);

        public static SpecialSeed Define(string name, int seed) => new(name, seed);

        public readonly int Index;
        public readonly string Name;
        public readonly int Seed;

        public SpecialSeed(string name, int seed)
        {
            Name = name;
            Seed = seed;
            _specialSeed[name] = this;
        }
    }
}
