using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.StoreValidators
{
    internal class UpdateProductSubscriptionValidator(IValidationRuleService validate, IUnitOfWorkRead uow) 
        : Validator<UpdateProductSubscriptionCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(UpdateProductSubscriptionCommand command)
        {
            await RuleFor().Transaction().IsExistAsync(command.ProductId,
                _uow.Store().IsExistProductAsync);
            if (IsInvalid) return GetErrors();

            var subscription = await _uow.Store()
                .GetSubscriptionAsync(command.ProductId,command.Type);

            RuleFor(p => p.IsActive).Transaction()
                .IsHaveAccess(subscription, command.IsActive);

            return GetErrors();
        }
    }
}
