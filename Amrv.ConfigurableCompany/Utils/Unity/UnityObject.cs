using UnityEngine;

namespace Amrv.ConfigurableCompany.Utils.Unity
{
    public class UnityObject
    {
        public static UnityObject Create(string name, GameObject parent = null)
        {
            UnityObject obj = new(name);
            if (parent != null)
            {
                obj.SetParent(parent);
            }
            return obj;
        }

        private readonly GameObject _gameObject;

        private UnityObject(string name)
        {
            _gameObject = new(name);
        }

        public UnityObject AddComponent<T>(bool dontAddIfExists = true) where T : Component
        {
            return AddComponent(out T _, dontAddIfExists);
        }

        public UnityObject AddComponent<T>(out T component, bool dontAddIfExists = true) where T : Component
        {
            if (dontAddIfExists && _gameObject.TryGetComponent(out component))
                return this;

            component = _gameObject.AddComponent<T>();
            return this;
        }

        public UnityObject SetParent(GameObject parent, bool moveToParent = true)
        {
            _gameObject.transform.SetParent(parent.transform, !moveToParent);

            _gameObject.layer = parent.layer;

            return this;
        }

        public UnityObject SetParent(Transform parent, bool moveToParent = true)
        {
            _gameObject.transform.SetParent(parent, !moveToParent);
            return this;
        }

        public GameObject GetObject()
        {
            return _gameObject;
        }

        public static implicit operator GameObject(UnityObject obj)
        {
            return obj.GetObject();
        }
    }
}
