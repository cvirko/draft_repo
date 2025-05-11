using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.PaymentValidators
{
    internal class PayForOrderValidator(IValidationRuleService validate,
        IUnitOfWorkRead uow) 
        : Validator<PayForOrderCommand, PayForOrderResponse>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(PayForOrderCommand command)
        {
            RuleFor(p => p.Type).Transaction()
                .IsNotContains(command.Type, 
                PaymentType.None, 
                PaymentType.Stripe_ProductPrice, 
                PaymentType.Stripe_Invoice);
            
            RuleFor().User().IsLengthFormatValid(command.UserId);
            
            if (IsInvalid) return GetErrors();

            await RuleFor().Transaction().IsExistAsync(command.UserId,
                _uow.Store().IsExistCollectingOrderAsync);
            return GetErrors();
        }
    }
}
