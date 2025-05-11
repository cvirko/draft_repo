using Auth.Domain.Core.Logic.Commands.Payments;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.PaymentValidators
{
    internal class CreateProductSubscriptionValidator(IValidationRuleService validate, IUnitOfWorkRead uow) 
        : Validator<CreateProductSubscriptionCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(CreateProductSubscriptionCommand command)
        {
            RuleFor(p => p.Type).Transaction()
                .IsContains(command.Type, PaymentType.Stripe_ProductPrice);

            RuleFor().User().IsLengthFormatValid(command.UserId);

            if (IsInvalid) return GetErrors();

            await RuleFor().Transaction().IsExistAsync(command.ProductId,
                _uow.Store().IsExistProductAsync);

            return GetErrors();
        }
    }
}
