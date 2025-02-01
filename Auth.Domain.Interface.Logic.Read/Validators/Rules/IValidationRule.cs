namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IValidationRule: IValidationRuleBase
    {
        bool IsLengthFormatValid(string value);
    }
}
