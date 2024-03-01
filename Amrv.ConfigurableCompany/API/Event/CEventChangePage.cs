namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventChangePage(CPage page) : CEvent
    {
        public readonly CPage Page = page;
    }
}
