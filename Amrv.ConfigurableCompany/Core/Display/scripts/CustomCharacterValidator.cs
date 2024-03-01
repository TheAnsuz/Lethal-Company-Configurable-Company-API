using TMPro;

namespace Amrv.ConfigurableCompany.Core.Display.Scripts
{
    public class CustomCharacterValidator(CustomCharacterValidator.CharValidator validator = null) : TMP_InputValidator
    {
        private const char NO_CHAR = '\0';

        public delegate bool CharValidator(ref string text, ref int pos, ref char ch);

        public CharValidator Validator = validator;

        public override char Validate(ref string text, ref int pos, char ch)
        {
            return Validator?.Invoke(ref text, ref pos, ref ch) ?? true ? ch : NO_CHAR;
        }
    }
}
