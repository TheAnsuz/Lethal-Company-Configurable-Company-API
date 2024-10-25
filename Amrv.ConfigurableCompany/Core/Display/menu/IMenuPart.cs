using System.Collections;

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
        public IEnumerator UpdateContent();

        /// <summary>
        /// Called to validate the container itself and check what state should it display
        /// </summary>
        public IEnumerator UpdateSelf();
    }

    internal static class IMenuPartExtension
    {

        /// <summary>
        /// Executes the UpdateContent IEnumerator as a full function, allows for function calling of the mentioned method
        /// </summary>
        public static void UpdateContentFull(this IMenuPart part)
        {
            IEnumerator enumerator = part.UpdateContent();
            while (enumerator.MoveNext()) { }
        }

        /// <summary>
        /// Executes the UpdateSelf IEnumerator as a full function, allows for function calling of the mentioned method
        /// </summary>
        public static void UpdateSelfFull(this IMenuPart part)
        {
            IEnumerator enumerator = part.UpdateSelf();
            while (enumerator.MoveNext()) { }
        }
    }
}
