using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.StoreValidators
{
    internal class AddProductValidator(IValidationRuleService validate, IUnitOfWorkRead uow) 
        : Validator<AddProductCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(AddProductCommand command)
        {
            await RuleFor(p => p.Name).Transaction()
                .IsNotExistAsync(command.Name, 
                _uow.Store().IsExistProductNameAsync);

            RuleFor(p => p.Price).Balance().IsPositive(command.Price);
            return GetErrors();
        }
    }
}
