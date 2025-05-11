using Auth.Domain.Core.Logic.Commands.Payments;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.PaymentValidators
{
    internal class UpdateWalletValidator(IValidationRuleService validate, IUnitOfWorkRead uow) 
        : Validator<UpdateWalletCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(UpdateWalletCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.ToUserId);
            RuleFor().Balance().IsLengthFormatValid(command.Amount);
            if (!IsInvalid)
                return GetErrors();

            await RuleFor(p => p.Amount).Balance()
                .IsEnoughFundsAsync(command.Amount, command.ToUserId,
                _uow.Users().GetExpectedBalanceAsync);

            return GetErrors();
        }
    }
}
