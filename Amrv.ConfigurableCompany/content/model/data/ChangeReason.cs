namespace Amrv.ConfigurableCompany.content.model.data
{
    public enum ChangeReason
    {
        /// <summary>
        /// The value has been set because it has been syncronized with the host
        /// </summary>
        SYNCHRONIZED,
        /// <summary>
        /// Somewhere in the code there was a call that requested the configuration to change for unknown reasons
        /// </summary>
        SCRIPTED,
        /// <summary>
        /// A final user modified it from the ingame menu
        /// </summary>
        USER_CHANGED,
        /// <summary>
        /// A final user requested the reset of this configuration
        /// </summary>
        USER_RESET,
        /// <summary>
        /// A reset has been requested somewhere in the code for an unknown reason
        /// </summary>
        SCRIPTED_RESET,
        /// <summary>
        /// The value has been set as the configuration constructor was called and a default value was set
        /// </summary>
        CONFIGURATION_CREATED,
        /// <summary>
        /// The value was set from the existing value inside the stored file data
        /// </summary>
        READ_FROM_FILE,
        /// <summary>
        /// The value was set from the user's clipboard after an imported configuration
        /// </summary>
        READ_FROM_CLIPBOARD,

    }

    public static class ChangeReasonExtension
    {

        public static API.ChangeReason NewReason(this ChangeReason reason)
        {
            return reason switch
            {
                ChangeReason.SYNCHRONIZED => API.ChangeReason.SYNCHRONIZATION,
                ChangeReason.USER_CHANGED => API.ChangeReason.USER_CHANGE,
                ChangeReason.USER_RESET => API.ChangeReason.USER_RESET,
                ChangeReason.SCRIPTED => API.ChangeReason.SCRIPT_CHANGE,
                ChangeReason.SCRIPTED_RESET => API.ChangeReason.SCRIPT_RESET,
                ChangeReason.CONFIGURATION_CREATED => API.ChangeReason.CREATION,
                ChangeReason.READ_FROM_FILE => API.ChangeReason.READ_FROM_FILE,
                ChangeReason.READ_FROM_CLIPBOARD => API.ChangeReason.PASTE_FROM_CLIPBOARD,
                _ => API.ChangeReason.SCRIPT_CHANGE,
            };
        }

        public static ChangeReason OldReason(this API.ChangeReason reason)
        {
            return reason switch
            {
                API.ChangeReason.SYNCHRONIZATION => ChangeReason.SYNCHRONIZED,
                API.ChangeReason.USER_CHANGE => ChangeReason.USER_CHANGED,
                API.ChangeReason.USER_RESET => ChangeReason.USER_RESET,
                API.ChangeReason.SCRIPT_CHANGE => ChangeReason.SCRIPTED,
                API.ChangeReason.SCRIPT_RESET => ChangeReason.SCRIPTED_RESET,
                API.ChangeReason.CREATION => ChangeReason.CONFIGURATION_CREATED,
                API.ChangeReason.READ_FROM_FILE => ChangeReason.READ_FROM_FILE,
                API.ChangeReason.PASTE_FROM_CLIPBOARD => ChangeReason.READ_FROM_CLIPBOARD,
                _ => ChangeReason.SCRIPTED,
            };
        }
    }
}
