using Auth.Domain.Core.Logic.Commands;
using Auth.Domain.Interface.Logic.Read.Validators.Rules;
using Library.CommandMediator.Interfaces;

namespace Auth.Domain.Interface.Logic.Read.Validators
{
    public interface IValidationRuleService : IBaseValidationRuleService<ValidationError, ErrorStatus>
    {
        IBalanceValidationRule Balance();
        IEmailValidationRule Email();
        INameValidationRule Name();
        IPasswordValidationRule Password();
        IUserValidationRule User();
        ITokenValidationRule Token();
        IFileValidationRule File();
        IMessageValidationRule Message();
        ITransactionValidationRule Transaction();
    }
}
