namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class MessageValidationRule(RegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule(regex, action), IMessageValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, nameof(UnitOfWorkValidationRule.Message)))
                return false;

            return true;
        }
    }
}
