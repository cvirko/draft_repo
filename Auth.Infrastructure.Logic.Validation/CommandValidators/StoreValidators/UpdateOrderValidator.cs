using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.StoreValidators
{
    internal class UpdateOrderValidator(IValidationRuleService validate, 
        IUnitOfWorkRead uow)
        : Validator<UpdateOrderCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(UpdateOrderCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            await RuleFor().Transaction().IsExistAsync(command.ProductIds.Keys, 
                _uow.Store().IsExistAciveProductsAsync);
            foreach (var amount in command.ProductIds.Values)
                RuleFor().Balance().IsPositive(amount);
            return GetErrors();
        }
    }
}
