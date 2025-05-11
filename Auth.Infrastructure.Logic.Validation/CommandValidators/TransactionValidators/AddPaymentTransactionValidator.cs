using Auth.Domain.Core.Logic.Commands.Transactions;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.TransactionValidators
{
    internal class AddPaymentTransactionValidator(IValidationRuleService validate) 
        : Validator<AddPaymentTransactionCommand, Guid>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(AddPaymentTransactionCommand command)
        {
            RuleFor(p => p.Type).Transaction()
                .IsNotContains(command.Type, PaymentType.None);

            RuleFor().User().IsLengthFormatValid(command.UserId);
            
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
