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
        READ_FROM_FILE
    }
}
