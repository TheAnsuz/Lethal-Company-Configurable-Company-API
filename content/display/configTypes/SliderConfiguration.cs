using Amrv.ConfigurableCompany.content.display.customObject;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public class SliderConfiguration : ConfigurationItemDisplay
    {
        public override int Height => 40;
        public const float SLIDER_HEIGHT = 10;
        public const float SLIDER_PADDING_LEFT = 10;
        public const float SLIDER_PADDING_RIGHT = 15;
        public const float SLIDER_HANDLE_WIDTH = 20;
        public static readonly Color SLIDER_HANDLE_BORDER_COLOR = Color.black;

        protected readonly GameObject Label;
        protected readonly RectTransform Label_Rect;
        protected readonly TextMeshProUGUI Label_Text;

        protected readonly GameObject InputArea;
        protected readonly RectTransform InputArea_Rect;
        protected readonly Slider InputArea_Slider;

        protected readonly GameObject SliderArea;
        protected readonly BorderGameObject SliderBackground;
        protected readonly GameObject SliderFill;
        protected readonly GameObject SliderHandle;
        protected readonly GameObject SliderValueDisplay;
        protected readonly TextMeshProUGUI SliderValueDisplay_Text;

        public SliderConfiguration(Configuration Config, float min, float max) : base(Config)
        {
            Label = UnityObject.Create("Label")
                .SetParent(Container)
                .AddComponent(out Label_Rect)
                .AddComponent(out Label_Text);

            Label_Rect.anchorMin = new(0, .5f);
            Label_Rect.anchorMax = new(1, 1);
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

            InputArea = UnityObject.Create("Input")
                .SetParent(Container)
                .AddComponent(out InputArea_Rect)
                .AddComponent(out InputArea_Slider);

            InputArea_Rect.anchorMin = new(0, 0);
            InputArea_Rect.anchorMax = new(1, .5f);
            InputArea_Rect.offsetMin = new(0, 0);
            InputArea_Rect.offsetMax = new(0, 0);

            SliderArea = UnityObject.Create("Slider fill area")
                .SetParent(InputArea_Rect)
                .AddComponent(out RectTransform SliderArea_Rect);

            SliderArea_Rect.anchorMin = new(0, .5f);
            SliderArea_Rect.anchorMax = new(1, .5f);
            SliderArea_Rect.offsetMin = new(SLIDER_PADDING_LEFT, 0);
            SliderArea_Rect.offsetMax = new(-SLIDER_PADDING_RIGHT, 0);

            SliderArea_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SLIDER_HEIGHT);

            SliderFill = UnityObject.Create("Slider fill")
                .SetParent(SliderArea_Rect)
                .AddComponent(out RectTransform SliderFill_Rect)
                .AddComponent(out Image SliderFill_Image);

            SliderFill_Rect.offsetMin = new(0, 2);
            SliderFill_Rect.offsetMax = new(0, -2);

            SliderFill_Image.color = BORDER_COLOR;

            SliderBackground = new("Slider background", SliderArea)
            {
                Size = BORDER_SIZE,
                Color = BORDER_COLOR,
                Padding = BORDER_PADDING
            };

            SliderHandle = UnityObject.Create("Slider handle")
                .SetParent(SliderArea_Rect)
                .AddComponent(out RectTransform SliderHandle_Rect)
                .AddComponent(out Outline SliderHandle_Outline)
                .AddComponent(out Image SliderHandle_Image);

            SliderHandle_Rect.offsetMin = Vector2.zero;
            SliderHandle_Rect.offsetMax = Vector2.zero;

            SliderHandle_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SLIDER_HANDLE_WIDTH);

            SliderHandle_Image.color = BORDER_COLOR;

            SliderHandle_Outline.effectDistance = new(1.75f, -1.75f);
            SliderHandle_Outline.effectColor = SLIDER_HANDLE_BORDER_COLOR;

            InputArea_Slider.colors = DisplayUtils.COLOR_TINT_DEFAULT;
            InputArea_Slider.navigation = DisplayUtils.NO_NAVIGATION;
            InputArea_Slider.fillRect = SliderFill_Rect;
            InputArea_Slider.handleRect = SliderHandle_Rect;
            InputArea_Slider.minValue = min;
            InputArea_Slider.maxValue = max;
            InputArea_Slider.value = Config.Get(0f);

            InputArea_Slider.onValueChanged.AddListener(OnValueChanged);

            SliderValueDisplay = UnityObject.Create("Text")
                .SetParent(SliderHandle_Rect)
                .AddComponent(out RectTransform SliderValueDisplay_Rect)
                .AddComponent(out SliderValueDisplay_Text);

            SliderValueDisplay_Rect.anchorMin = new(0, 0);
            SliderValueDisplay_Rect.anchorMax = new(1, 1);
            SliderValueDisplay_Rect.offsetMin = new(0, 0);
            SliderValueDisplay_Rect.offsetMax = new(0, 0);

            SliderValueDisplay_Text.color = Config.NeedsRestart ? DisplayUtils.COLOR_INPUT_TEXT_REQUIRE_RESTART_INVERTED : DisplayUtils.COLOR_INPUT_TEXT_INVERTED;
            SliderValueDisplay_Text.fontStyle = FontStyles.Bold;
            SliderValueDisplay_Text.font = DisplayUtils.GAME_FONT;
            SliderValueDisplay_Text.fontSize = 4;
            SliderValueDisplay_Text.fontSizeMin = 2;
            SliderValueDisplay_Text.enableAutoSizing = true;
            SliderValueDisplay_Text.enableWordWrapping = false;
            SliderValueDisplay_Text.overflowMode = TextOverflowModes.Truncate;
            SliderValueDisplay_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            SliderValueDisplay_Text.verticalAlignment = VerticalAlignmentOptions.Middle;

            SliderValueDisplay_Text.SetText(Math.Round(InputArea_Slider.value) + "");
        }

        private void OnValueChanged(float newValue)
        {
            SliderValueDisplay_Text.SetText(Math.Round(InputArea_Slider.value) + "");
        }

        protected override void GetFromConfig(Configuration Config)
        {
            InputArea_Slider.value = Config.Get(0f);
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(InputArea_Slider.value, model.data.ChangeReason.USER_CHANGED);
        }

        public override void RefreshConfig()
        {
            Label_Text.SetText(Config.Name);
        }

        protected override void OnClick()
        {

        }
    }
}
