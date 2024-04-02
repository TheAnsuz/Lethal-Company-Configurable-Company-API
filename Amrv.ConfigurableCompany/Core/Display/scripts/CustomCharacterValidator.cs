using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.Core.Display.Scripts
{
    public class CustomCharacterValidator : TMP_InputValidator
    {
        public static TMP_InputValidator Create(CharValidator validator)
        {
            CustomCharacterValidator ccv = CreateInstance<CustomCharacterValidator>();
            ccv.Validator = validator;
            return ccv;
        }

        private const char NO_CHAR = '\0';

        public delegate bool CharValidator(ref string text, ref int pos, ref char ch);

        public CharValidator Validator;

        public override char Validate(ref string text, ref int pos, char ch)
        {
            return Validator?.Invoke(ref text, ref pos, ref ch) ?? true ? ch : NO_CHAR;
        }
    }
}
