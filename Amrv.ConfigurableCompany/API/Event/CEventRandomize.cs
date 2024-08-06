using Amrv.ConfigurableCompany.API.Data;
using System;

namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventRandomize(RNGProvider rng, InfoProvider info) : CEvent
    {
        public readonly RNGProvider Random = rng;
        public readonly InfoProvider Info = info;
    }
}