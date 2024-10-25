using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Extensions
{
    public static class GameObjectExtensions
    {
        //[Obsolete("Use GetChild")]
        public static GameObject FindChild(this GameObject gameObject, string child)
        {
            return gameObject.transform.Find(child)?.gameObject ?? null;
        }

        public static GameObject GetChild(this GameObject gameObject, int index)
        {
            return gameObject.transform.GetChild(index)?.gameObject ?? null;
        }

        public static GameObject GetChild(this GameObject gameObject, params int[] indexes)
        {
            Transform transform = gameObject.transform;

            for (int i = 0; i < indexes.Length; i++)
                transform = transform?.GetChild(indexes[i]) ?? null;

            return transform?.gameObject ?? null;
        }

        public static void AddComponent<T>(this GameObject gameObject, out T addedComponent) where T : Component
        {
            addedComponent = gameObject.AddComponent<T>();
        }
    }
}
