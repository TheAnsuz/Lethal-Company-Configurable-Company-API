﻿using Amrv.ConfigurableCompany.content.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.display.configTypes
{
    public class StringConfiguration : LargeInputConfiguration
    {
        public StringConfiguration(Configuration Config) : base(Config, TMP_InputField.ContentType.Standard)
        {
        }

        protected override void GetFromConfig(Configuration Config)
        {
            InputArea_Input.text = Config.Get<string>();
        }

        protected override void SetToConfig(Configuration Config)
        {
            Config.TrySet(InputArea_Input.text, model.data.ChangeReason.USER_CHANGED);
        }
    }
}
