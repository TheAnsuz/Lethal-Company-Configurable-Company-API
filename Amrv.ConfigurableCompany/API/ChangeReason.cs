namespace Amrv.ConfigurableCompany.API
{
    public enum ChangeReason
    {
        /// <summary>
        /// The value was received from the host and has been synchronized
        /// </summary>
        SYNCHRONIZATION = 1,
        /// <summary>
        /// The user requested to reset the configuration from the GUI
        /// </summary>
        USER_RESET = 2,
        /// <summary>
        /// Somewhere an external mod decided to reset the configuration gracefully
        /// </summary>
        SCRIPT_RESET = 3,
        /// <summary>
        /// The value has been read from the file containing the configurations
        /// </summary>
        READ_FROM_FILE = 4,
        /// <summary>
        /// The value was set because the configuration was created
        /// </summary>
        CREATION = 5,
        /// <summary>
        /// The user wanted to change the configuration using the GUI
        /// </summary>
        USER_CHANGE = 6,
        /// <summary>
        /// Somewhere an external mod decided to change the configuration gracefully
        /// </summary>
        SCRIPT_CHANGE = 7,
        /// <summary>
        /// The configuration was pasted from the user's clipboard
        /// </summary>
        PASTE_FROM_CLIPBOARD = 8,
        /// <summary>
        /// Somewhere an external mod decided to randomize the configuration
        /// </summary>
        SCRIPT_RANDOMIZED = 9,
        /// <summary>
        /// The player requested the configuration to be randomized
        /// </summary>
        USER_RANDOMIZED = 10,
    }
}
