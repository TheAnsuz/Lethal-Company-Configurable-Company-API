using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Scripts
{
    public class LifecycleListener : MonoBehaviour
    {
        public event Action DestroyEvent;

        protected void OnDestroy()
        {
            DestroyEvent?.Invoke();
        }
    }
}
