namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventCreateSection(CSection section) : CEvent
    {
        public readonly CSection Section = section;
    }
}