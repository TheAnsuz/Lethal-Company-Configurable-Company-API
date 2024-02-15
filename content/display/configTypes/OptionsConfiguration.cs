using Amrv.ConfigurableCompany.content.display.customObject;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.model.types;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public class OptionsConfiguration : ConfigurationItemDisplay
    {
        public const float MARGIN = .05f;
        public const float SPLIT = .6f;
        public override int Height => 35;

        public string[] Options { get; private set; }

        private int _index;
        public int Index
        {
            get => _index;

            set
            {
                if (_index == -1)
                    throw new ArgumentException($"Tried to set index to -1");
                _index = value;
            }
        }

        public readonly GameObject Label;
        public readonly RectTransform Label_Rect;
        public readonly TextMeshProUGUI Label_Text;

        public readonly BorderGameObject ValueArea;
        public readonly GameObject Value;
        public readonly RectTransform Value_Rect;
        public readonly TextMeshProUGUI Value_Text;

        public readonly GameObject ArrowLeft;
        public readonly GameObject ArrowRight;

        public OptionsConfiguration(Configuration Config, params string[] options) : base(Config)
        {
            Options = options;
            Index = 0;

            Label = UnityObject.Create("Label")
                .SetParent(Container)
                .AddComponent(out Label_Rect)
                .AddComponent(out Label_Text);

            Label_Rect.anchorMin = new(0, SPLIT);
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
            Label_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            Label_Text.verticalAlignment = VerticalAlignmentOptions.Middle;

            Label_Text.SetText(Config.Name);

            ValueArea = new BorderGameObject("Area", Container);

            ValueArea.Transform.anchorMin = new(MARGIN, 0f);
            ValueArea.Transform.anchorMax = new(1f - MARGIN, SPLIT);
            ValueArea.Transform.offsetMin = new(0, 0);
            ValueArea.Transform.offsetMax = new(0, 0);

            ValueArea.Color = BORDER_COLOR;
            ValueArea.Size = BORDER_SIZE;
            ValueArea.Padding = BORDER_PADDING;

            Value = UnityObject.Create("Text")
                .SetParent(ValueArea)
                .AddComponent(out Value_Text)
                .AddComponent(out Value_Rect);

            Value_Rect.anchorMin = new(0, 0f);
            Value_Rect.anchorMax = new(1f, 1f);
            Value_Rect.offsetMin = new(0, 0);
            Value_Rect.offsetMax = new(0, 0);

            Value_Text.font = DisplayUtils.GAME_FONT;
            Value_Text.color = Config.NeedsRestart ? COLOR_INPUT_REQUIRE_RESTART : COLOR_INPUT_NORMAL;
            Value_Text.fontSize = FONT_SIZE_NORMAL;
            Value_Text.fontSizeMin = FONT_SIZE_MIN;
            Value_Text.fontSizeMax = FONT_SIZE_MAX;
            Value_Text.enableAutoSizing = true;
            Value_Text.autoSizeTextContainer = false;
            Value_Text.enableWordWrapping = false;
            Value_Text.overflowMode = TextOverflowModes.Truncate;
            Value_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            Value_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            Value_Text.margin = new(6, 0, 6, 0);
            Value_Text.SetText(Options[Index].ToString());

            ArrowLeft = UnityObject.Create("Left")
                .SetParent(Container_Rect)
                .AddComponent(out RectTransform ArrowLeft_Rect)
                .AddComponent(out Button ArrowLeft_Button)
                .AddComponent(out TextMeshProUGUI ArrowLeft_Text);

            ArrowLeft_Rect.anchorMin = new(0, 0);
            ArrowLeft_Rect.anchorMax = new(MARGIN, SPLIT);
            ArrowLeft_Rect.offsetMin = new(0, 0);
            ArrowLeft_Rect.offsetMax = new(0, 0);

            ArrowLeft_Text.fontStyle = FontStyles.Bold;
            ArrowLeft_Text.font = DisplayUtils.GAME_FONT;
            ArrowLeft_Text.fontSize = 18;
            ArrowLeft_Text.color = BORDER_COLOR;
            ArrowLeft_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            ArrowLeft_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            ArrowLeft_Text.margin = new(4, 0, 4, 0);
            ArrowLeft_Text.SetText("<");

            ArrowLeft_Button.onClick.AddListener(Previous);

            ArrowRight = UnityObject.Create("Right")
                .SetParent(Container_Rect)
                .AddComponent(out RectTransform ArrowRight_Rect)
                .AddComponent(out Button ArrowRight_Button)
                .AddComponent(out TextMeshProUGUI ArrowRight_Text);

            ArrowRight_Rect.anchorMin = new(1f - MARGIN, 0);
            ArrowRight_Rect.anchorMax = new(1f, SPLIT);
            ArrowRight_Rect.offsetMin = new(0, 0);
            ArrowRight_Rect.offsetMax = new(0, 0);

            ArrowRight_Text.fontStyle = FontStyles.Bold;
            ArrowRight_Text.font = DisplayUtils.GAME_FONT;
            ArrowRight_Text.fontSize = 18;
            ArrowRight_Text.color = BORDER_COLOR;
            ArrowRight_Text.verticalAlignment = VerticalAlignmentOptions.Middle;
            ArrowRight_Text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            ArrowRight_Text.margin = new(4, 0, 4, 0);
            ArrowRight_Text.SetText(">");

            ArrowRight_Button.onClick.AddListener(Next);
        }

        public void Next()
        {
            if (Index == -1)
                DisplayIndex(0);
            else if (Index + 1 < Options.Length)
                DisplayIndex(Index + 1);
            else
                DisplayIndex(0);
        }

        public void Previous()
        {
            if (Index == -1)
                DisplayIndex(Options.Length - 1);
            else if (Index - 1 >= 0)
                DisplayIndex(Index - 1);
            else
                DisplayIndex(Options.Length - 1);
        }

        public void DisplayIndex(int index)
        {
            Index = index;
            Value_Text.SetText(Options[Index].ToString());
        }

        public override void RefreshDisplay()
        {
            Label_Text.SetText(Config.Name);
        }

        protected override void GetFromConfig(Configuration Config)
        {
            if (Config.Type is ListConfigurationType type)
            {
                Options = type.StringList();

                if (Config.TryGet(out int index))
                    DisplayIndex(index);
            }
        }

        protected override void OnClick(object sender, PointerEventData e)
        {
            base.OnClick(sender, e);
            if (e.button == PointerEventData.InputButton.Left)
                Previous();
            else if (e.button == PointerEventData.InputButton.Right)
                Next();
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(Index, model.data.ChangeReason.USER_CHANGED, CultureInfo.CurrentCulture);
        }
    }
}
