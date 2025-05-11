using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.PaymentValidators
{
    internal class SubscriptionValidator(IValidationRuleService validate,
        IUnitOfWorkRead uow) 
        : Validator<SubscriptionCommand, SubscriptionResponse>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(SubscriptionCommand command)
        {  
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor(p => p.Type).Transaction()
                .IsContains(command.Type,
                    PaymentType.Stripe_SubscriptionSchedule
                );
            if (IsInvalid) return GetErrors();

            var subscription = await _uow.Store().GetSubscriptionAsync(command.ProductId, command.Type);
            RuleFor(p => p.Type).Transaction().IsExist(subscription, p => p.IsActive);
            return GetErrors();
        }
    }
}
