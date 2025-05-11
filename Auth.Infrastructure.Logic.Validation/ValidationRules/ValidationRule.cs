namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal abstract class ValidationRule<T>(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRuleBase(action)
    {
        protected readonly IRegexService _regex = regex;
        public abstract bool IsLengthFormatValid(T value);
        protected bool IsLengthInvalid(string value, Range length)
        {
           
            if (_regex.IsFieldInvalidLength(value, length))
            {
                AddError(ErrorStatus.Length, length.Start, length.End);
                return true;
            }
            return false;
        }
    }
}
