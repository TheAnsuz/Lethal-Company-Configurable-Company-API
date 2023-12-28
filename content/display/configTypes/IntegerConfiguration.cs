﻿using Amrv.ConfigurableCompany.content.model;
using TMPro;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public sealed class IntegerConfiguration : SmallInputConfiguration
    {
        public IntegerConfiguration(Configuration Config) : base(Config, TMP_InputField.ContentType.IntegerNumber)
        {
        }

        protected override void GetFromConfig(Configuration Config)
        {
            InputArea_Input.text = Config.Value.ToString();
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(InputArea_Input.text, model.data.ChangeReason.USER_CHANGED);
        }
    }
}
