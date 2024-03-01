namespace Amrv.ConfigurableCompany.API.Event
{
    public static class CEvents
    {
        public static class MenuEvents
        {
            /// <summary>
            /// Event triggered when the save button is clicked
            /// </summary>
            public static readonly CEventType<CEvent> Save = new();
            /// <summary>
            /// Event triggered when the user requests to reset configurations
            /// </summary>
            public static readonly CEventType<CEvent> Reset = new();
            public static readonly CEventType<CEvent> Restore = new();
            public static readonly CEventType<CEvent> Copy = new();
            public static readonly CEventType<CEvent> Paste = new();
            public static readonly CEventType<CEvent> Create = new();
            public static readonly CEventType<CEvent> Prepare = new();
            public static readonly CEventType<CEvent> Destroy = new();
            public static readonly CEventType<CEventMenuToggle> Toggle = new();
            public static readonly CEventType<CEventMenuVisible> Visible = new();
            public static readonly CEventType<CEventChangePage> ChangePage = new();
        }

        public static class ConfigEvents
        {
            public static readonly CEventType<CEventCreatePage> CreatePage = new();
            public static readonly CEventType<CEventCreateCategory> CreateCategory = new();
            public static readonly CEventType<CEventCreateSection> CreateSection = new();
            public static readonly CEventType<CEventCreateConfig> CreateConfig = new();
            public static readonly CEventType<CEventChangeConfig> ChangeConfig = new();
            public static readonly CEventType<CEventToggleConfig> ToggleConfig = new();
        }

        public static class LifecycleEvents
        {
            public static readonly CEventType<CEvent> PluginStart = new();
        }

        public static class IOSEvents
        {
            public static readonly CEventType<CEventCopyClipboard> CopyToClipboard = new();
            public static readonly CEventType<CEventPasteClipboard> PasteFromClipboard = new();
        }
    }
}
