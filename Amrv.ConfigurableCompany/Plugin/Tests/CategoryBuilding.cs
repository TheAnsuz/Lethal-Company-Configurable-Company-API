using Amrv.ConfigurableCompany.API;
using HarmonyLib;
using System;
using UnityEngine;

#if DEBUG
namespace Amrv.ConfigurableCompany.Plugin.Tests
{
    [HarmonyPatch(typeof(SaveFileUISlot))]
    internal static class CategoryBuilding
    {
        public static void Ping() { }

        private static int index = 0;

        [HarmonyPatch(nameof(SaveFileUISlot.SetFileToThis))]
        [HarmonyPostfix]
        public static void AddPageOnFileClick()
        {
            Console.WriteLine($"Creating dynamic category");
            using CCategoryBuilder builder = new();
            builder.SetID($"configurable-company_test-category-dynamic_cat-{index++}");
            builder.SetName("Category created dynamically");
            builder.SetPage(PageBuilding.Page_Normal);
            builder.SetColor(Color.yellow);
            builder.Build();
        }

        public static CCategory Normal = new CCategoryBuilder()
        {
            Page = PageBuilding.Page_Normal,
            ID = "configurable-company_test-category_normal",
            Color = Color.blue,
            Name = "Normal category",
        };

        public static CCategory LargeName = new CCategoryBuilder()
        {
            Page = PageBuilding.Page_LargeInfo,
            ID = "configurable-company_test-category_large-name",
            Color = Color.cyan,
            Name = "Normal category that has a very long name that should exceed the limit of characters so the name should not appear complete in the interface",
        };

        public static CCategory NullID = new CCategoryBuilder()
        {
            Page = PageBuilding.Page_NullID,
            Color = Color.gray,
            Name = "Null id category",
        };

        public static CCategory NullData = new CCategoryBuilder()
        {
            Page = PageBuilding.Page_NullData,
            Color = Color.gray,
            ID = "configurable-company_test-category_null-data",
        };
    }
}
#endif