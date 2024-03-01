namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventMenuVisible(bool visible) : CEvent
    {
        public readonly bool Visible = visible;
    }
}
