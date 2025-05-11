namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IValidationRule<T>: IValidationRuleBase
    {
        bool IsLengthFormatValid(T value);
    }
}
