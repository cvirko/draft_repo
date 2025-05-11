using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.TransactionValidators
{
    internal class CkeckPaymentTransactionValidator(IValidationRuleService validate) 
        : Validator<CheckPaymentTransactionCommand, CheckPaymentTransactionResponse>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(CheckPaymentTransactionCommand command)
        {
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
