using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.Cache;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class CreateAccountFromCacheValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow,
        ICacheRepository cache) 
        : Validator<CreateAccountFromCacheCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly ICacheRepository _cache = cache;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(CreateAccountFromCacheCommand command)
        {
            RuleFor(p => p.Email).Email().IsLengthFormatValid(command.Email);
            if (IsInvalid()) return GetErrors();
            if (await RuleFor(p => p.Email).Email()
                .IsNotOccupiedAsync(command.Email, _uow.Users().IsExistEmailAsync))
            {
                await RuleFor(p => p.Email).Email()
                .IsExistAsync(_cache.GetSignUpEmailKey(command.Email),
                                _cache.IsExistDataAsync);
            }

            return GetErrors();
        }
    }
}
