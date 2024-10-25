using Amrv.ConfigurableCompany.Core.Display.Menu;
using Amrv.ConfigurableCompany.Core.Display.Scripts;
using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    public class MenuLoader
    {
        private static GameObject _canvasObject;
        private static Canvas _canvas;
        private static CanvasScaler _canvasScaler;

        private static MenuLoader _instance;

        public static MenuLoader GetInstance()
        {
            if (_instance == null)
                Create();

            return _instance;
        }

        protected internal static MenuLoader Create()
        {
            ConfigurableCompanyPlugin.Debug($"Creating Menu Loader");

            if (_instance != null)
                return _instance;

            ConfigurableCompanyPlugin.Debug($"Created Menu Loader");

            _canvasObject = new GameObject("ConfigurableCompanyCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(LifecycleListener));

            UnityEngine.Object.DontDestroyOnLoad(_canvasObject);

            _canvas = _canvasObject.GetComponent<Canvas>();
            _canvasScaler = _canvasObject.GetComponent<CanvasScaler>();

            _canvasObject.GetComponent<LifecycleListener>().DestroyEvent += Destroy;

            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.pixelPerfect = true;
            _canvas.sortingOrder = 1;

            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            _canvasScaler.scaleFactor = 1.55f;
            _canvasScaler.referencePixelsPerUnit = 1;

            _instance = new MenuLoader(UnityEngine.Object.Instantiate(MenuPrefabs.Loader, _canvas.transform, false))
            {
                Visible = false
            };

            return _instance;
        }

        protected internal static void Destroy()
        {
            _instance?.DestroyInstance();
            _instance = null;
            UnityEngine.Object.Destroy(_canvasObject);
            _canvas = null;
            _canvasObject = null;
            _canvasScaler = null;
        }

        protected readonly GameObject Object;
        protected readonly TextMeshProUGUI TitleText;
        protected readonly TextMeshProUGUI InfoText;
        protected readonly Image FillImage;

        public string Title
        {
            get => TitleText.text;
            set => TitleText.SetText(value);
        }

        public string Text
        {
            get => InfoText.text;
            set => InfoText.SetText(value);
        }

        public float Fill
        {
            get => FillImage.fillAmount;
            set => FillImage.fillAmount = value;
        }

        public bool Visible
        {
            get => Object.activeSelf;
            set => Object.SetActive(value);
        }

        private MenuLoader(GameObject loader)
        {
            Object = loader;
            TitleText = Object.FindChild("Title").GetComponent<TextMeshProUGUI>();
            InfoText = Object.FindChild("Text").GetComponent<TextMeshProUGUI>();
            FillImage = Object.FindChild("Filler").GetComponent<Image>();
        }

        private void DestroyInstance()
        {
            if (Object != null)
                UnityEngine.Object.Destroy(Object);
        }
    }
}
