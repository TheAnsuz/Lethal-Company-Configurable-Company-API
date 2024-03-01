namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventCreatePage(CPage page) : CEvent
    {
        public readonly CPage Page = page;
    }
}
