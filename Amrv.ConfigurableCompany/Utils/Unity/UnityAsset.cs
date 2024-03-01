using UnityEngine;

namespace Amrv.ConfigurableCompany.Utils.Unity
{
    internal class UnityAsset
    {
        public static FastBundle GetBundle(string folder, string name)
        {
            return new FastBundle(AssetBundle.LoadFromFile(folder + name));
        }

        public static T LoadAsset<T>(string folder, string name, string assetPath) where T : UnityEngine.Object
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(folder + name);

            if (bundle == null)
            {
                return null;
            }

            T asset = bundle.LoadAsset<T>(assetPath.ToLower());

            bundle.UnloadAsync(false);
            return asset;
        }

        public static T FindAsset<T>(string name) where T : UnityEngine.Object
        {
            T[] resources = Resources.FindObjectsOfTypeAll<T>();

            if (resources == null || resources.Length == 0)
                return null;

            foreach (T resource in resources)
                if (resource.name.Contains(name))
                    return resource;

            return resources[0];
        }

        private UnityAsset() { }
    }
}
