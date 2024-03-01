namespace Amrv.ConfigurableCompany.API.Event
{
    public class CEventChangeConfig(CConfig config, ChangeReason reason, object oldValue, object requestedValue, bool succeded, bool converted) : CEvent
    {
        public readonly CConfig Config = config;
        public readonly ChangeReason Reason = reason;
        public readonly bool Success = succeded;
        public readonly bool Converted = converted;
        public readonly object OldValue = oldValue;
        public readonly object RequestedValue = requestedValue;
        public readonly object NewValue = config.Value;
    }
}