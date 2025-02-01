namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class NameValidationRule(RegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule(regex, action), INameValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, nameof(UnitOfWorkValidationRule.Name)))
                return false;

            if (_regex.IsNotOnlyNumbersLowercaseUppercaseLatin(value))
            {
                AddError(ErrorStatus.Format);
                return false;
            }
            return true;
        }
    }
}
