using ConfigurableCompany.display.item;
using ConfigurableCompany.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConfigurableCompany.utils
{
    public class DisplayUtils
    {
        private DisplayUtils() { }

        public const int TOOLTIP_MAX_LINE_LENGTH = 64;
        public const int TOOLTIP_MAX_SPACE_DISPLACEMENT = 10;

        public static readonly ColorBlock COLOR_TINT_DEFAULT = new()
        {
            colorMultiplier = 1f,
            fadeDuration = .2f,
            normalColor = new Color32(180, 180, 180, 255),
            highlightedColor = new Color32(250, 250, 250, 255),
            pressedColor = new Color32(250, 250, 250, 255)
        };

        public static readonly Color COLOR_ENTRY_BACKGROUND = new Color32(110, 0, 0, 255);

        public static readonly Color COLOR_VIEWPORT_BACKGROUND = new Color32(70, 5, 5, 255);
        public static readonly Color COLOR_VIEWPORT_OUTLINE = new Color32(180, 40, 48, 255);

        public static readonly Color COLOR_SCROLLBAR_BACKGROUND = new Color32(95, 0, 0, 255);
        public static readonly Color COLOR_SCROLLBAR_HANDLE = new Color32(115, 55, 0, 255);
        public static readonly Color COLOR_SCROLLBAR_HANDLE_OUTLINE = new Color32(255, 125, 0, 255);

        public static readonly Color COLOR_INPUT_TEXT = new Color32(255, 184, 117, 255);
        public static readonly Color COLOR_INPUT_BACKGROUND = new Color32(125, 0, 0, 255);
        public static readonly Color COLOR_INPUT_SELECTION = new Color32(237, 152, 24, 120);

        public static readonly Color COLOR_LABEL_TEXT = new Color32(255, 184, 117, 255);
        public static readonly Color COLOR_LABEL_BACKGROUND = new Color32(110, 0, 0, 255);

        public static readonly Color COLOR_TOOLTIP_BACKGROUND = new Color32(64, 0, 0, 255);
        public static readonly Color COLOR_TOOLTIP_TEXT = new Color32(255, 184, 117, 255);
        public static readonly Color COLOR_TOOLTIP_OUTLINE = new Color32(255, 184, 117, 255);

        private static readonly TMP_FontAsset[] _fontAssets = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();

        public static void LoadFontAssets()
        {
            foreach (TMP_FontAsset asset in _fontAssets)
                if (asset.name.Contains("3270-Regular"))
                {
                    GAME_FONT = asset;
                    return;
                }

            GAME_FONT = _fontAssets.LastOrDefault();
        }

        public static TMP_FontAsset GAME_FONT { get; private set; }

        public static readonly int GAME_AVG_SIZE = 8;

        public static readonly LayerMask UI_LAYER = LayerMask.NameToLayer("UI");

        public static void RegisterTypes()
        {
            ConfigurationItemDisplay.RegisterIfMissing(ConfigurationType.String, (menu, parent, config) => new StringItemDisplay(menu, parent, config));
            ConfigurationItemDisplay.RegisterIfMissing(ConfigurationType.Boolean, (menu, parent, config) => new BooleanItemDisplay(menu, parent, config));
            ConfigurationItemDisplay.RegisterIfMissing(ConfigurationType.Integer, (menu, parent, config) => new IntegerItemDisplay(menu, parent, config));
            ConfigurationItemDisplay.RegisterIfMissing(ConfigurationType.Float, (menu, parent, config) => new FloatItemDisplay(menu, parent, config));
            ConfigurationItemDisplay.RegisterIfMissing(ConfigurationType.Percent, (menu, parent, config) => new SliderItemDisplay(menu, parent, config));
        }

        public static string FixTooltipSize(string tooltip)
        {
            StringBuilder builder = new();

            string[] lines = tooltip.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                // Remove unnecesary spaces
                lines[i] = lines[i].Trim();

                // If the line exceeds the maximum character length
                if (lines[i].Length > TOOLTIP_MAX_LINE_LENGTH)
                {
                    int sublines = 1 + lines[i].Length / TOOLTIP_MAX_LINE_LENGTH;
                    int offset = 0;
                    for (int l = 0; l < sublines; l++)
                    {
                        int startIndex = l * TOOLTIP_MAX_LINE_LENGTH + offset;
                        int length = Math.Min(lines[i].Length - startIndex, TOOLTIP_MAX_LINE_LENGTH);

                        int nearSpaceForwardIndex = lines[i].IndexOf(' ', startIndex + length - 1);
                        int nearSpaceBacktrackIndex = lines[i].LastIndexOf(' ', startIndex + length - 1);

                        int nearSpaceForwardDistance = nearSpaceForwardIndex - (startIndex + length);
                        int nearSpaceBacktrackDistance = (startIndex + length) - nearSpaceBacktrackIndex;

                        if (nearSpaceForwardDistance < nearSpaceBacktrackDistance)
                        {
                            if (nearSpaceForwardDistance > 0 && nearSpaceForwardDistance < TOOLTIP_MAX_SPACE_DISPLACEMENT)
                            {
                                offset += nearSpaceForwardDistance;
                                length += nearSpaceForwardDistance;
                            }
                        }
                        else
                        {
                            if (nearSpaceBacktrackDistance > 0 && nearSpaceBacktrackDistance < TOOLTIP_MAX_SPACE_DISPLACEMENT)
                            {
                                offset -= nearSpaceBacktrackDistance;
                                length -= nearSpaceBacktrackDistance;
                            }
                        }

                        /*
                         * A freaking lot of debugging to make this work correctly
                        ConfigurableCompanyPlugin.Info($"String: {lines[i]}");
                        ConfigurableCompanyPlugin.Info($"Start index: {startIndex}");
                        ConfigurableCompanyPlugin.Info($"Length: {length} [I: {startIndex + length}]");
                        ConfigurableCompanyPlugin.Info($"Nearest space forward: {nearSpaceForwardIndex} [{nearSpaceForwardDistance}]");
                        ConfigurableCompanyPlugin.Info($"Nearest space backtrack: {nearSpaceBacktrackIndex} [{nearSpaceBacktrackDistance}]");
                        ConfigurableCompanyPlugin.Info($"Substring: {lines[i].Substring(startIndex, length)}");
                        ConfigurableCompanyPlugin.Info($"Remaining: {lines[i].Length - (startIndex + length)}");
                        */

                        builder.Append(lines[i].Substring(startIndex, length).Trim());
                        builder.Append('\n');
                    }

                }
                else
                    builder.Append(lines[i]).Append('\n');

            }

            return builder.ToString();
        }
    }
}
