using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Utils.Unity
{
    public class FastBundle(AssetBundle bundle) : IDisposable
    {
        private readonly AssetBundle Bundle = bundle;
        private string path = "";

        public string Path { get => path; set => path = value.ToLower(); }

        public void In(string segment)
        {
            path += segment.ToLower();
        }
        public void OutAll() => path = string.Empty;
        public void Out()
        {
            Out("/");
        }
        public void Out(string segment)
        {
            int index = path.LastIndexOf(segment);

            if (index != -1)
            {
                path = path[..index];
            }
        }

        [Obsolete("use GetAllAssetNames")]
        public string[] AllAssetNames() => Bundle.AllAssetNames();
        public string[] GetAllAssetNames() => Bundle.GetAllAssetNames();
        public string[] GetAllScenePaths() => Bundle.GetAllScenePaths();

        public override bool Equals(object other) => Bundle.Equals(other);
        public bool Contains(string name) => Bundle.Contains(name);
        public override int GetHashCode() => Bundle.GetHashCode();
        public IntPtr GetCachedPtr() => Bundle.GetCachedPtr();
        public int GetInstanceID() => Bundle.GetInstanceID();
        public override string ToString() => Bundle.ToString();

        public void EnsureRunningOnMainThread() => Bundle.EnsureRunningOnMainThread();

        public T[] LoadAllAssets<T>() where T : UnityEngine.Object => Bundle.LoadAllAssets<T>();
        public AssetBundleRequest LoadAllAssetsAsync<T>() where T : UnityEngine.Object => Bundle.LoadAllAssetsAsync<T>();

        public T LoadAsset<T>(string name) where T : UnityEngine.Object => Bundle.LoadAsset<T>(path + name);
        public AssetBundleRequest LoadAssetAsync<T>(string name) where T : UnityEngine.Object => Bundle.LoadAssetAsync<T>(path + name.ToLower());
        public UnityEngine.Object LoadAsset_Internal(string name, Type type) => Bundle.LoadAsset_Internal(path + name.ToLower(), type);
        //public AssetBundleRequest LoadAssetAsync_Internal(string name, Type type) => Bundle.LoadAssetAsync_Internal(path + name.ToLower(), type);
        public bool TryLoadAsset<T>(in string name, out T asset) where T : UnityEngine.Object
        {
            asset = LoadAsset<T>(name);
            return asset != null;
        }

        public T[] LoadAssetWithSubAssets<T>(string name) where T : UnityEngine.Object => Bundle.LoadAssetWithSubAssets<T>(path + name.ToLower());
        //public UnityEngine.Object[] LoadAssetWithSubAssets_Internal(string name, Type type) => Bundle.LoadAssetWithSubAssets_Internal(path + name.ToLower(), type);
        public AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string name) where T : UnityEngine.Object => Bundle.LoadAssetWithSubAssetsAsync<T>(path + name.ToLower());
        //public AssetBundleRequest LoadAssetWithSubAssetsAsync_Internal(string name, Type type) => Bundle.LoadAssetWithSubAssetsAsync_Internal(path + name.ToLower(), type);
        public bool LoadAssetWithSubAssets<T>(in string name, out T[] assets) where T : UnityEngine.Object
        {
            assets = LoadAssetWithSubAssets<T>(name);
            return assets != null && assets.Length != 0;
        }

        public void Unload(bool unloadAllLoadedObjects) => Bundle.Unload(unloadAllLoadedObjects);
        public void UnloadAsync(bool unloadAllLoadedObjects) => Bundle.UnloadAsync(unloadAllLoadedObjects);

        public void MarkDirty() => Bundle.MarkDirty();

        public void Dispose() => Bundle.Unload(false);
    }
}
