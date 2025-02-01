namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal abstract class ValidationRule(RegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRuleBase(action)
    {
        protected readonly RegexService _regex = regex;
        public abstract bool IsLengthFormatValid(string value);
        protected bool IsLengthInvalid(string value, string name)
        {
            var lengthValue = _regex.Length[name];
           
            if (_regex.IsFieldInvalidLength(value, lengthValue))
            {
                AddError(ErrorStatus.Length, lengthValue.Start, lengthValue.End);
                return true;
            }
            return false;
        }
    }
}
