using Amrv.ConfigurableCompany.Core.Extensions;
using Amrv.ConfigurableCompany.Plugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    public class MenuLoader
    {
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

            _instance = new MenuLoader(UnityEngine.Object.Instantiate(MenuPrefabs.Loader, GlobalCanvas.Instance.transform, false))
            {
                Visible = false
            };

            return _instance;
        }

        protected internal static void Destroy()
        {
            _instance?.DestroyInstance();
            _instance = null;
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
