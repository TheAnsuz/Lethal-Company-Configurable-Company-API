using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.API.Data
{
    public sealed class InfoProvider
    {
        private static readonly InfoChallenge DEFAULT_INFO_CHALLENGE = new(new DateTime(ticks: 1234567890, DateTimeKind.Utc));

        public static readonly InfoProvider Default = new("");

        public readonly string Seed;
        public readonly bool IsDefault = false;
        public readonly InfoChallenge Challenge = DEFAULT_INFO_CHALLENGE;
        public readonly bool IsChallenge = false;
        public readonly SpecialSeed SpecialSeed = null;
        public readonly bool IsSpecialSeed = false;

        private InfoProvider(string seed)
        {
            Seed = seed;
            IsDefault = true;
        }

        internal InfoProvider(string seed, InfoChallenge challenge)
        {
            Seed = seed;
            Challenge = challenge;
            IsChallenge = true;
        }

        internal InfoProvider(string seed, SpecialSeed specialSeed)
        {
            Seed = seed;
            SpecialSeed = specialSeed;
            IsSpecialSeed = true;
        }
    }
}
