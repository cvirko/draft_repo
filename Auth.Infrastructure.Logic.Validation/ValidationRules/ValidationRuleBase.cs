namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal abstract class ValidationRuleBase(Action<ErrorStatus, object[]> action)
    {
        protected readonly Action<ErrorStatus, object[]> _action = action;

        protected void AddError(ErrorStatus status, params object[] values) => _action(status, values);

    }
}
