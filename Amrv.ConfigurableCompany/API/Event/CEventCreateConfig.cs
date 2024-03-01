namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventCreateConfig(CConfig config) : CEvent
    {
        public readonly CConfig Config = config;
    }
}