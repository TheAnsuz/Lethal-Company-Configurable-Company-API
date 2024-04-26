using System;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventRandomize(int seed, Random random) : CEvent
    {
        public readonly int Seed = seed;
        public readonly Random Random = random;
    }
}