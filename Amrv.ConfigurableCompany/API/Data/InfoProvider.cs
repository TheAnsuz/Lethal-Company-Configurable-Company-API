namespace Amrv.ConfigurableCompany.API.Data
{
    public sealed class InfoProvider
    {
        public static readonly InfoProvider Default = new();

        public readonly string SeedString;

        public readonly bool UseDefault;

        public readonly InfoChallenge Challenge;
        public readonly SpecialSeed SpecialSeed;

        public readonly bool IsChallenge;
        public readonly bool IsSpecialSeed;

        private InfoProvider()
        {
            SeedString = "DEFAULT";
            IsSpecialSeed = false;
            IsChallenge = false;
            SpecialSeed = null;
            Challenge = InfoChallenge.Default;
            UseDefault = true;
        }

        internal InfoProvider(string seedString)
        {
            SeedString = seedString;
            IsSpecialSeed = false;
            IsChallenge = false;
            SpecialSeed = null;
            Challenge = InfoChallenge.Default;
            UseDefault = false;
        }

        internal InfoProvider(string seedString, SpecialSeed specialSeed)
        {
            SeedString = seedString;
            IsSpecialSeed = true;
            IsChallenge = false;
            SpecialSeed = specialSeed;
            Challenge = InfoChallenge.Default;
            UseDefault = specialSeed.Seed == 0;
        }

        internal InfoProvider(string seedString, InfoChallenge challenge)
        {
            SeedString = seedString;
            IsSpecialSeed = false;
            IsChallenge = true;
            Challenge = challenge;
            SpecialSeed = null;
            UseDefault = false;
        }

        /*
        private InfoProvider(string seed, long seedNumber)
        {
            Seed = seed;
            SeedNumber = seedNumber;
            IsDefault = seedNumber == 0;

            ConfigurableCompanyPlugin.Debug($"Created normal info provider (seed: {Seed}| isDefault: {IsDefault})");
        }

        internal InfoProvider(string seed, long seedNumber, InfoChallenge challenge)
        {
            Seed = seed;
            Challenge = challenge;
            IsChallenge = true;
            SeedNumber = seedNumber;
            IsDefault = seedNumber == 0;
            ConfigurableCompanyPlugin.Debug($"Created challenge info provider (seed: {Seed}| isDefault: {IsDefault})");
        }

        internal InfoProvider(string seed, long seedNumber, SpecialSeed specialSeed)
        {
            Seed = seed;
            SpecialSeed = specialSeed;
            IsSpecialSeed = true;
            SeedNumber = seedNumber;
            IsDefault = seedNumber == 0;
            ConfigurableCompanyPlugin.Debug($"Created custom info provider (seed: {Seed}| isDefault: {IsDefault})");
        }
        */
    }
}
