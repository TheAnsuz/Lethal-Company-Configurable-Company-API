using Amrv.ConfigurableCompany.Core.Display.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display
{
    public static class GlobalCanvas
    {
        public static Canvas Instance
        {
            get
            {
                lock (_creationLock)
                {
                    if (!_created || _canvas == null)
                    {
                        Destroy();
                        Create();
                    }
                    return _canvas;
                }
            }
        }

        private static GameObject _canvasObject;
        private static Canvas _canvas;
        private static CanvasScaler _canvasScaler;
        private static EventSystem _eventSystem;
        private static GraphicRaycaster _raycaster;

        private static object _creationLock = new();
        private static bool _created = false;

        public static void Create()
        {
            lock (_creationLock)
            {
                if (_created) return;

                _canvasObject = new GameObject("ConfigurableCompanyCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(LifecycleListener), typeof(GraphicRaycaster));

                UnityEngine.Object.DontDestroyOnLoad(_canvasObject);

                _canvas = _canvasObject.GetComponent<Canvas>();
                _canvasScaler = _canvasObject.GetComponent<CanvasScaler>();
                //_eventSystem = _canvasObject.GetComponent<EventSystem>();
                _raycaster = _canvasObject.GetComponent<GraphicRaycaster>();

                _canvasObject.GetComponent<LifecycleListener>().DestroyEvent += Destroy;

                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _canvas.pixelPerfect = true;
                _canvas.sortingOrder = 0;

                _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                _canvasScaler.scaleFactor = 1.55f;
                _canvasScaler.referencePixelsPerUnit = 1;

                _raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.TwoD;

                _created = true;
            }
        }

        public static void Destroy()
        {
            lock (_creationLock)
            {
                if (!_created) return;

                UnityEngine.Object.Destroy(_canvasObject);
                _canvas = null;
                _canvasObject = null;
                _canvasScaler = null;

                _created = false;
            }
        }

    }
}
