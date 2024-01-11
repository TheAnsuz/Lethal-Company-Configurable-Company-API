using Amrv.ConfigurableCompany.content.model;
using TMPro;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public abstract class LargeInputConfiguration : SmallInputConfiguration
    {
        public override int Height => 50;

        protected LargeInputConfiguration(Configuration Config, TMP_InputField.ContentType contentType, int charLimit) : base(Config, contentType)
        {
            Label_Rect.anchorMin = new(0, .5f);
            Label_Rect.anchorMax = new(1, 1);
            Label_Rect.offsetMin = new(0, 0);
            Label_Rect.offsetMax = new(0, 0);

            InputArea_Rect.anchorMin = new(0, 0);
            InputArea_Rect.anchorMax = new(1, .5f);
            InputArea_Rect.offsetMin = new(0, 0);
            InputArea_Rect.offsetMax = new(0, 0);

            InputValue_Text.margin = new(3, 0, 0, 0);

            InputArea_Input.characterLimit = charLimit;
        }
    }
}
