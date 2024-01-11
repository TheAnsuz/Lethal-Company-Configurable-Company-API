using Amrv.ConfigurableCompany.content.model;
using TMPro;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public class StringConfiguration : LargeInputConfiguration
    {
        public StringConfiguration(Configuration Config, int maxCharacters) : base(Config, TMP_InputField.ContentType.Standard, maxCharacters)
        {
        }

        protected override void GetFromConfig(Configuration Config)
        {
            base.GetFromConfig(Config);
            InputArea_Input.text = Config.Value.ToString();
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(InputArea_Input.text, model.data.ChangeReason.USER_CHANGED);
        }
    }
}
