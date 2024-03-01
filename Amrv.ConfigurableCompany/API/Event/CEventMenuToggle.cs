namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventMenuToggle(bool isOpen) : CEvent
    {
        public readonly bool IsOpen = isOpen;
    }
}
