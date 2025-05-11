using Auth.Domain.Core.Logic.Commands.Payments;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.PaymentValidators
{
    internal class ConfirmPaymentActionValidator(IValidationRuleService validate,
        IUnitOfWorkRead uow) 
        : Validator<ConfirmPaymentActionCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(ConfirmPaymentActionCommand command)
        {
            RuleFor(p => p.PaymentId).Message().IsLengthFormatValid(command.PaymentId);
            RuleFor(p => p.Type).Transaction().IsLengthFormatValid(command.TransactionId);
            RuleFor(p => p.Type).Transaction().IsNotContains(command.Type, PaymentType.None);

            if (IsInvalid) return GetErrors();

            await RuleFor().Transaction().IsHaveAccessAsync(
                _uow.Transaction().IsPendingAsync, command.TransactionId);

            return GetErrors();
        }
    }
}
