using TMPro;

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
            if (Validator?.Invoke(ref text, ref pos, ref ch) ?? true)
            {
                text += ch;
                return ch;
            }
            return NO_CHAR;
        }
    }
}
