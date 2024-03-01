using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils.Unity;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal static class MenuPresets
    {
        public static readonly GameObject Menu;
        public static readonly GameObject Page;
        public static readonly GameObject Tag;
        public static readonly GameObject Category;
        public static readonly GameObject Section;

        public static readonly GameObject Config_Input;
        public static readonly GameObject Config_LargeInput;
        public static readonly GameObject Config_Bool;
        public static readonly GameObject Config_Slider;
        public static readonly GameObject Config_Enum;
        public static readonly GameObject Config_DoubleSlider;
        public static readonly GameObject Config_DoubleInput;

        static MenuPresets()
        {
            using FastBundle bundle = UnityAsset.GetBundle(ConfigurableCompanyPlugin.PluginFolder, "configuration_menu");

            bundle.Path = "Assets/ConfigurationMenu/";

            Menu = bundle.LoadAsset<GameObject>("Configuration Menu.prefab");
            Page = bundle.LoadAsset<GameObject>("Configuration Page.prefab");
            Tag = bundle.LoadAsset<GameObject>("Configuration Tag.prefab");
            Category = bundle.LoadAsset<GameObject>("Configuration Category.prefab");
            Section = bundle.LoadAsset<GameObject>("Configuration Section.prefab");

            Config_Input = bundle.LoadAsset<GameObject>("ConfigType Input.prefab");
            Config_LargeInput = bundle.LoadAsset<GameObject>("ConfigType LargeInput.prefab");
            Config_Bool = bundle.LoadAsset<GameObject>("ConfigType Bool.prefab");
            Config_Slider = bundle.LoadAsset<GameObject>("ConfigType Slider.prefab");
            Config_Enum = bundle.LoadAsset<GameObject>("ConfigType Enum.prefab");
            Config_DoubleSlider = bundle.LoadAsset<GameObject>("ConfigType DoubleSlider.prefab");
            Config_DoubleInput = bundle.LoadAsset<GameObject>("ConfigType DoubleInput.prefab");
        }

        internal static void Ping() { }
    }
}
