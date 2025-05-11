namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class MessageValidationRule(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule<string>(regex, action), IMessageValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, new Range(1, 256)))
                return false;

            return true;
        }
    }
}
