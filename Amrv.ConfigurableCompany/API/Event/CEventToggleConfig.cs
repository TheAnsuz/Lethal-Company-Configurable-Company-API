namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventToggleConfig(CConfig config, bool enabled) : CEvent
    {
        public readonly CConfig Config = config;
        public readonly bool Enabled = enabled;
    }
}