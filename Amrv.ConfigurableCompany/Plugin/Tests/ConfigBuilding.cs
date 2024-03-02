using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.API.ConfigTypes;
using GameNetcodeStuff;
using HarmonyLib;

#if DEBUG
namespace Amrv.ConfigurableCompany.Plugin.Tests
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class ConfigBuildDynamicTransmit
    {

        [HarmonyPatch("OpenMenu_performed")]
        [HarmonyPostfix]
        private static void Method()
        {
            ConfigBuilding.IntegerAutomatic.TrySet(ConfigBuilding.IntegerAutomatic.Get(10) + 1);
        }

    }

    [HarmonyPatch(typeof(SaveFileUISlot))]
    internal static class ConfigBuilding
    {
        public static void Ping() { }

        private static int id = 0;
        [HarmonyPatch(nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        private static void SetFileToThis_Postfix()
        {
            using CConfigBuilder builder = new CConfigBuilder();
            builder.CSection = SectionBuilding.Normal;
            builder.ID = "configurable-company-test_config_number-dynamic" + (id++);
            builder.Value = id;
            builder.Tooltip = "Simple tooltip for dynamic numeric configuration";
        }

        public static CConfig Boolean = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_boolean",
            Name = "Boolean",
            Tooltip = "Simple boolean configuration",
            Value = true
        };

        public static CConfig SliderInt = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_int-slider",
            Name = "Slider integer",
            Tooltip = "Simple integer slider configuration",
            Value = 10,
            Type = CTypes.WholePercent()
        };

        public static CConfig List = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_list",
            Name = "Value list for single options",
            Tooltip = "Only one option at a time",
            Value = "Option b",
            Type = new ArraySingleType("Option a", "Option b", "Option c")
        };

        public static CConfig Tuple = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_tuple1",
            Name = "Value tuple",
            Tooltip = "Range of values",
            Value = (15, 20),
        };

        public static CConfig TupleDecimal = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_tuple2",
            Name = "Decimal value tuple",
            Tooltip = "Range of decimal values",
            Value = (15f, 20f),
        };

        public static CConfig String = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_string",
            Name = "Text normal",
            Tooltip = "Normal text configuration",
            Value = "hola",
            Synchronized = true,
            Toogleable = true,
        };

        public static CConfig Enum = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_enum",
            Name = "Slider integer",
            Tooltip = "Simple integer slider configuration",
            Value = LevelWeatherType.DustClouds,
            Type = CTypes.EnumSinlgeOption<LevelWeatherType>(),
            Toogleable = true
        };

        public static CConfig SliderDecimal = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_float-slider",
            Name = "Slider float",
            Tooltip = "Simple float slider configuration",
            Value = 10,
            Type = CTypes.DecimalPercent(),
            Synchronized = true,
        };

        public static CConfig FloatingNumber = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_floating-number",
            Name = "Floating number",
            Tooltip = "Simple floating number configuration",
            Type = new DecimalType(),
            Value = 10
        };

        public static CConfig IntegerNumber = new CConfigBuilder()
        {
            CCategory = CategoryBuilding.Normal,
            ID = "configurable-company-test_config_integer-number",
            Name = "Integer number",
            Tooltip = "Simple integer number configuration",
            Type = new IntegerType(),
            Value = 10
        };

        public static CConfig IntegerNumberRanged = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_integer-number-range",
            Name = "Integer number",
            Tooltip = "Simple integer number configuration",
            Type = new IntegerType(0, 25),
            Value = 30
        };

        public static CConfig IntegerAutomatic = new CConfigBuilder()
        {
            CCategory = CategoryBuilding.Normal,
            ID = "configurable-company-test_config_integer-number-synchronized",
            Name = "Integer number (synchronized)",
            Tooltip = "Simple synchronized integer number configuration",
            Type = new IntegerType(),
            Value = 0
        };

        public static CConfig AutoDetected = new CConfigBuilder()
        {
            CSection = SectionBuilding.Normal,
            ID = "configurable-company-test_config_auto-detected",
            Name = "Auto detected",
            Tooltip = "Auto detected type configuration",
            Value = 10f
        };
    }
}
#endif