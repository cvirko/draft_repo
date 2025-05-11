namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class NameValidationRule(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule<string>(regex, action), INameValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, new Range(2, 32)))
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
