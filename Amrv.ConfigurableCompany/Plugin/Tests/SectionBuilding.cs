using Amrv.ConfigurableCompany.API;
using HarmonyLib;
using System;

#if DEBUG
namespace Amrv.ConfigurableCompany.Plugin.Tests
{
    [HarmonyPatch(typeof(SaveFileUISlot))]

    internal static class SectionBuilding
    {
        public static void Ping() { }

        private static int index = 0;
        [HarmonyPatch(nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        public static void AddPageOnFileClick()
        {
            Console.WriteLine($"Creating dynamic section");
            using CSectionBuilder builder = new();
            builder.SetID($"configurable-company_section_dynamic-test-{index++}");
            builder.SetName("Section created dynamically");
            builder.SetCategory(CategoryBuilding.Normal);
            builder.Build();
        }

        public static CSection Normal = new CSectionBuilder()
        {
            CCategory = CategoryBuilding.Normal,
            ID = "configurable-company_section_normal",
            Name = "Normal section"
        };

        public static CSection LargeName = new CSectionBuilder()
        {
            CCategory = CategoryBuilding.Normal,
            ID = "configurable-company_section_large-name",
            Name = "Normal section with a very long name that should exceed the limit of characters thus causing an elipse in the text interface"
        };

        public static CSection NullID = new CSectionBuilder()
        {
            CCategory = CategoryBuilding.Normal,
            Name = "Null section id"
        };
    }
}
#endif