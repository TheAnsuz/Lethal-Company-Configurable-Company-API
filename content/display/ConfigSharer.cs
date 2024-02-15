using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using Amrv.ConfigurableCompany.display.component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display
{
    public class ConfigSharer
    {
        protected readonly ConfigurationScreen ConfigurationScreen;

        protected readonly GameObject ImportArea;
        protected readonly GameObject ImportObject;
        protected readonly GameObject ExportArea;
        protected readonly GameObject ExportObject;

        internal ConfigSharer(GameObject sharingArea, ConfigurationScreen configurationScreen)
        {
            ConfigurationScreen = configurationScreen;

            ImportArea = UnityObject.Create("Import")
                .SetParent(sharingArea)
                .AddComponent(out RectTransform ImportArea_Rect)
                .AddComponent(out Image ImportArea_Image)
                .AddComponent(out Outline ImportArea_Outline)
                .AddComponent(out RegionButton ImportArea_RegionButton);

            ImportArea_Rect.anchorMin = new(0, 0);
            ImportArea_Rect.anchorMax = new(.49f, 1);
            ImportArea_Rect.offsetMin = new(0, 0);
            ImportArea_Rect.offsetMax = new(0, 0);

            ImportArea_Image.color = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;
            ImportArea_Outline.effectColor = DisplayUtils.COLOR_VIEWPORT_OUTLINE;

            ImportArea_RegionButton.OnMouseClick += delegate
            {
                ConfigurationShare.CopyToClipboard();
            };

            ImportObject = UnityObject.Create("Text")
                .SetParent(ImportArea_Rect)
                .AddComponent(out RectTransform ImportObject_Rect)
                .AddComponent(out TextMeshProUGUI ImportObject_Text);

            ImportObject_Rect.anchorMin = new(0, 0);
            ImportObject_Rect.anchorMax = new(1, 1);
            ImportObject_Rect.offsetMin = new(0, 0);
            ImportObject_Rect.offsetMax = new(0, 0);

            ImportObject_Text.font = DisplayUtils.GAME_FONT;
            ImportObject_Text.margin = new(2, 2, 2, 2);
            ImportObject_Text.fontSize = 12;
            ImportObject_Text.fontSizeMax = 18;
            ImportObject_Text.fontSizeMin = 8;
            ImportObject_Text.enableAutoSizing = true;
            ImportObject_Text.color = new Color32(255, 154, 66, 255);
            ImportObject_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            ImportObject_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            ImportObject_Text.enableWordWrapping = false;

            ImportObject_Text.SetText("Copy to clipboard");

            ExportArea = UnityObject.Create("Export")
                .SetParent(sharingArea)
                .AddComponent(out RectTransform ExportArea_Rect)
                .AddComponent(out Image ExportArea_Image)
                .AddComponent(out Outline ExportArea_Outline)
                .AddComponent(out RegionButton ExportArea_RegionButton);

            ExportArea_Rect.anchorMin = new(.51f, 0);
            ExportArea_Rect.anchorMax = new(1, 1);
            ExportArea_Rect.offsetMin = new(0, 0);
            ExportArea_Rect.offsetMax = new(0, 0);

            ExportArea_Image.color = DisplayUtils.COLOR_VIEWPORT_BACKGROUND;
            ExportArea_Outline.effectColor = DisplayUtils.COLOR_VIEWPORT_OUTLINE;

            ExportArea_RegionButton.OnMouseClick += delegate
            {
                ConfigurationShare.PasteFromClipboard();
                ConfigurationScreen.LoadAll();
            };

            ExportObject = UnityObject.Create("Text")
                .SetParent(ExportArea_Rect)
                .AddComponent(out RectTransform ExportObject_Rect)
                .AddComponent(out TextMeshProUGUI ExportObject_Text);

            ExportObject_Rect.anchorMin = new(0, 0);
            ExportObject_Rect.anchorMax = new(1, 1);
            ExportObject_Rect.offsetMin = new(0, 0);
            ExportObject_Rect.offsetMax = new(0, 0);

            ExportObject_Text.font = DisplayUtils.GAME_FONT;
            ExportObject_Text.margin = new(2, 2, 2, 2);
            ExportObject_Text.fontSize = 12;
            ExportObject_Text.fontSizeMax = 18;
            ExportObject_Text.fontSizeMin = 8;
            ExportObject_Text.enableAutoSizing = true;
            ExportObject_Text.color = new Color32(255, 154, 66,255);
            ExportObject_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            ExportObject_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            ExportObject_Text.enableWordWrapping = false;

            ExportObject_Text.SetText("Paste from clipboard");
        }
    }
}
