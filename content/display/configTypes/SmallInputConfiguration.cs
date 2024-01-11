using Amrv.ConfigurableCompany.content.display.customObject;
using Amrv.ConfigurableCompany.content.model;
using Amrv.ConfigurableCompany.content.unity;
using Amrv.ConfigurableCompany.content.utils;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public abstract class SmallInputConfiguration : ConfigurationItemDisplay
    {
        public override int Height => 25;
        public virtual float VALUE_PADDING => 2;

        protected readonly GameObject Label;
        protected readonly RectTransform Label_Rect;
        protected readonly TextMeshProUGUI Label_Text;

        protected readonly GameObject InputArea;
        protected readonly RectTransform InputArea_Rect;
        protected readonly TMP_InputField InputArea_Input;

        protected readonly GameObject InputViewport;
        protected readonly RectTransform InputViewport_Rect;

        protected readonly GameObject InputValue;
        protected readonly RectTransform InputValue_Rect;
        protected readonly TextMeshProUGUI InputValue_Text;

        protected readonly BorderGameObject InputBorder;

        protected string _oldText;

        public SmallInputConfiguration(Configuration Config, TMP_InputField.ContentType contentType) : base(Config)
        {
            Label = UnityObject.Create("Label")
                .SetParent(Container)
                .AddComponent(out Label_Rect)
                .AddComponent(out Label_Text);

            Label_Rect.anchorMin = new(0, 0);
            Label_Rect.anchorMax = new(.7f, 1);
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
                .AddComponent(out InputArea_Input);

            InputArea_Rect.anchorMin = new(.7f, 0);
            InputArea_Rect.anchorMax = new(1, 1);
            InputArea_Rect.offsetMin = new(0, 0);
            InputArea_Rect.offsetMax = new(0, 0);

            InputViewport = UnityObject.Create("Viewport")
                .SetParent(InputArea_Rect)
                .AddComponent(out InputViewport_Rect);

            InputViewport_Rect.anchorMin = new(0, 0);
            InputViewport_Rect.anchorMax = new(1, 1);
            InputViewport_Rect.offsetMin = new(BORDER_PADDING, BORDER_PADDING);
            InputViewport_Rect.offsetMax = new(-BORDER_PADDING, -BORDER_PADDING);

            InputBorder = new("Border", InputViewport)
            {
                Color = BORDER_COLOR,
                Size = BORDER_SIZE
            };

            InputValue = UnityObject.Create("Text")
                .SetParent(InputViewport_Rect)
                .AddComponent(out InputValue_Rect)
                .AddComponent(out InputValue_Text);

            InputValue_Rect.anchorMin = new(0, 0);
            InputValue_Rect.anchorMax = new(1, 1);
            InputValue_Rect.offsetMin = new(0, 0);
            InputValue_Rect.offsetMax = new(0, 0);

            InputValue_Text.font = DisplayUtils.GAME_FONT;
            InputValue_Text.color = Config.NeedsRestart ? COLOR_INPUT_REQUIRE_RESTART : COLOR_INPUT_NORMAL;
            InputValue_Text.fontSize = FONT_SIZE_NORMAL;
            InputValue_Text.fontSizeMin = FONT_SIZE_MIN;
            InputValue_Text.fontSizeMax = FONT_SIZE_MAX;
            InputValue_Text.enableAutoSizing = true;
            InputValue_Text.enableWordWrapping = false;
            InputValue_Text.overflowMode = TextOverflowModes.Truncate;
            InputValue_Text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            InputValue_Text.verticalAlignment = VerticalAlignmentOptions.Middle;

            InputArea_Input.navigation = DisplayUtils.NO_NAVIGATION;
            InputArea_Input.textViewport = InputViewport_Rect;
            InputArea_Input.textComponent = InputValue_Text;
            InputArea_Input.lineLimit = 1;
            InputArea_Input.characterLimit = 10;
            InputArea_Input.enabled = false;
            InputArea_Input.enabled = true;
            InputArea_Input.restoreOriginalTextOnEscape = false;
            InputArea_Input.customCaretColor = true;
            InputArea_Input.caretColor = DisplayUtils.COLOR_INPUT_CARET;
            InputArea_Input.caretWidth = 1;
            InputArea_Input.onEndEdit.AddListener(OnEditEnd);
            InputArea_Input.onSubmit.AddListener(OnSubmit);
            InputArea_Input.onDeselect.AddListener(OnDeselect);
            InputArea_Input.contentType = contentType;

            InputValue_Text.margin = new(3, 0, 0, 0);

            _oldText = InputArea_Input.text;
        }

        protected virtual void OnSubmit(string text)
        {
            ConfigurableCompanyPlugin.Debug($"SmallInputConfiguration::OnSubmit");
            InputArea_Input.enabled = false;
            InputArea_Input.enabled = true;
        }

        protected virtual void OnEditEnd(string text)
        {
            if (!ValidateText(text))
                InputArea_Input.text = _oldText;

            _oldText = InputArea_Input.text;
            /*
            else
            {
                InputArea_Input.enabled = false;
                InputArea_Input.enabled = true;
                InputArea_Input.Select();
                InputArea_Input.ActivateInputField();
            }
            */
            ConfigurableCompanyPlugin.Debug($"SmallInputConfiguration::OnEditEnd [{_oldText} -- {InputArea_Input.text}]");
        }

        protected override void OnClick()
        {
            InputArea_Input.enabled = true;
            InputArea_Input.Select();
            InputArea_Input.ActivateInputField();
        }

        protected virtual bool ValidateText(string text) => true;

        protected virtual void OnDeselect(string text)
        {
            ConfigurableCompanyPlugin.Debug($"SmallInputConfiguration::OnDeselect");
        }

        public override void RefreshConfig()
        {
            Label_Text.SetText(Config.Name);
        }

        protected override void GetFromConfig(Configuration Config)
        {
            _oldText = Config.Value.ToString();
        }
    }
}
