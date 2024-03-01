using Amrv.ConfigurableCompany.API;
using HarmonyLib;
using System;

#if DEBUG
namespace Amrv.ConfigurableCompany.Plugin.Tests
{
    [HarmonyPatch(typeof(SaveFileUISlot))]
    internal static class PageBuilding
    {
        public static void Ping() { }

        public static CPage Page_Using;

        private static int index = 0;

        [HarmonyPatch(nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        public static void AddPageOnFileClick()
        {
            Console.WriteLine($"Creating dynamic page");
            using CPageBuilder builder = new();
            builder.SetID($"configurable-company-test_dynamic-page-{index++}");
            builder.SetName("Page created dynamically");
            builder.SetDescription("Page created after menu creation");
            builder.TryBuild(out Page_Using);
        }

        static PageBuilding()
        {
            using CPageBuilder builder = new();
            builder.SetID("configurable-company-test_page-using");
            builder.SetName("Page created with usings");
            builder.SetDescription("Page created with the using keyword");
            builder.TryBuild(out Page_Using);
        }

        public static CPage Page_Normal = new CPageBuilder()
        {
            ID = "configurable-company-test_page-normal",
            Name = "Page 1",
            Description = "Description about page 1"
        };

        public static CPage Page_LargeInfo = new CPageBuilder()
        {
            ID = "configurable-company-test_page-large-info",
            Name = "Page with too many characters in the name and the description",
            Description = "Long description about a page with many characters to try to overflow the space"
        };

        public static CPage Page_NullID = new CPageBuilder()
        {
            ID = null,
            Name = "Page null",
            Description = "Page null"
        };

        public static CPage Page_NullData = new CPageBuilder()
        {
            ID = "configurable-company-test_page-null-data",
            Name = null,
            Description = null
        };

        public static CPage Page_OtherBuild = CPage.Builder()
            .SetID("configurable-company-test_page-other-build")
            .SetDescription("Page built using another kind of building")
            .SetName("Page built using factory design pattern");
    }
}
#endif