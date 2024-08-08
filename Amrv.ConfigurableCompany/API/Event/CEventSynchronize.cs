namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventSynchronize(bool sent, params CConfig[] configs) : CEvent
    {
        public bool IsSender = sent;
        public bool IsReceiver = !sent;
        public CConfig[] Configs = configs ?? new CConfig[0];
    }
}