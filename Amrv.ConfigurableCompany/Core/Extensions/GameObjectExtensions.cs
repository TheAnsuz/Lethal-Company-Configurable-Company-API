using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject FindChild(this GameObject gameObject, string child)
        {
            return gameObject.transform.Find(child)?.gameObject ?? null;
        }

        public static void AddComponent<T>(this GameObject gameObject, out T addedComponent) where T : Component
        {
            addedComponent = gameObject.AddComponent<T>();
        }
    }
}
