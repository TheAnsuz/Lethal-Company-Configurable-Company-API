using Amrv.ConfigurableCompany.content.display.customObject;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public class BooleanConfiguration : ConfigurationItemDisplay
    {
        public const string VALUE_ON = "[ON]";
        public const string VALUE_OFF = "[OFF]";

        public override int Height => 25;

        protected readonly GameObject Label;
        protected readonly TextMeshProUGUI Label_Text;
        protected readonly GameObject Value;

        private readonly TextMeshProUGUI DisplayText_Text;
        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;

                if (value)
                    DisplayText_Text.SetText("[ON]");
                else
                    DisplayText_Text.SetText("[OFF]");
                _enabled = value;
            }
        }

        internal BooleanConfiguration(Configuration Config) : base(Config)
        {
            Label = UnityObject.Create("Label")
                .SetParent(Container)
                .AddComponent(out RectTransform Label_Rect)
                .AddComponent(out Label_Text);

            Label_Rect.anchorMin = new(0, 0);
            Label_Rect.anchorMax = new(.8f, 1);
            Label_Rect.offsetMin = new(0, 0);
            Label_Rect.offsetMax = new(0, 0);

            Label_Text.font = DisplayUtils.GAME_FONT;
            Label_Text.fontSize = FONT_SIZE_NORMAL;
            Label_Text.fontSizeMin = FONT_SIZE_MIN;
            Label_Text.fontSizeMax = FONT_SIZE_MAX;
            Label_Text.enableAutoSizing = true;
            Label_Text.enableWordWrapping = false;
            Label_Text.overflowMode = TextOverflowModes.Truncate;
            Label_Text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            Label_Text.verticalAlignment = VerticalAlignmentOptions.Middle;

            Label_Text.SetText(Config.Name);

            Value = UnityObject.Create("Value")
                .SetParent(Container)
                .AddComponent(out RectTransform Value_Rect);

            Value_Rect.anchorMin = new(.8f, 0);
            Value_Rect.anchorMax = new(1, 1);
            Value_Rect.offsetMin = new(BORDER_PADDING, BORDER_PADDING);
            Value_Rect.offsetMax = new(-BORDER_PADDING, -BORDER_PADDING);

            UnityObject.Create("Text")
                .SetParent(Value_Rect)
                .AddComponent(out RectTransform DisplayText_Rect)
                .AddComponent(out DisplayText_Text);

            DisplayText_Rect.anchorMin = new(0, 0);
            DisplayText_Rect.anchorMax = new(1, 1);
            DisplayText_Rect.offsetMin = new(0, 0);
            DisplayText_Rect.offsetMax = new(0, 0);

            DisplayText_Text.font = DisplayUtils.GAME_FONT;
            DisplayText_Text.color = Config.NeedsRestart ? COLOR_INPUT_REQUIRE_RESTART : COLOR_INPUT_NORMAL;
            DisplayText_Text.fontSize = FONT_SIZE_NORMAL;
            DisplayText_Text.fontSizeMin = FONT_SIZE_MIN;
            DisplayText_Text.fontSizeMax = FONT_SIZE_MAX;
            DisplayText_Text.enableAutoSizing = true;
            DisplayText_Text.enableWordWrapping = false;
            DisplayText_Text.overflowMode = TextOverflowModes.Truncate;
            DisplayText_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            DisplayText_Text.verticalAlignment = VerticalAlignmentOptions.Middle;

            BorderGameObject ValueBorder = new("Border")
            {
                Size = BORDER_SIZE,
                Color = BORDER_COLOR
            };
            ValueBorder.SetParent(Value_Rect);

            DisplayText_Text.SetText(Config.Get(false) ? VALUE_ON : VALUE_OFF);
            Enabled = Config.Get(false);
        }

        protected override void OnClick(object sender, PointerEventData e)
        {
            base.OnClick(sender, e);
            Enabled = !Enabled;
        }

        protected override void GetFromConfig(Configuration Config)
        {
            Enabled = Config.Get(false);
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(Enabled, model.data.ChangeReason.USER_CHANGED, CultureInfo.CurrentCulture);
        }

        public override void RefreshDisplay()
        {
            Label_Text.SetText(Config.Name);
        }
    }
}
