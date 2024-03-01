namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventCreateCategory(CCategory category) : CEvent
    {
        public readonly CCategory Category = category;
    }
}