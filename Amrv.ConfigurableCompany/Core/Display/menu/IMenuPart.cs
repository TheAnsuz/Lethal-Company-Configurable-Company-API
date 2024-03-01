namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal interface IMenuPart
    {
        /// <summary>
        /// Called when the menu will no longer be used.
        /// Should clear all resources, call other destroy methods and/or destroy game objects.
        /// Once this method is called, the object should not be accesed again
        /// </summary>
        public void Destroy();

        /// <summary>
        /// Called to validate the contents of the panel and fill/refill the information that this panel holds
        /// </summary>
        public void UpdateContent();

        /// <summary>
        /// Called to validate the container itself and check what state should it display
        /// </summary>
        public void UpdateSelf();
    }
}
