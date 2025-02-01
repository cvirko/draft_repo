using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.Cache;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class SignUpInCacheValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow,
        ICacheRepository cache) 
        : Validator<SignUpInCacheCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly ICacheRepository _cache = cache;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(SignUpInCacheCommand command)
        {
            RuleFor(p => p.Email).Email().IsLengthFormatValid(command.Email);
            RuleFor(p => p.Password).Password().IsLengthFormatValid(command.Password);
            RuleFor(p => p.UserName).Name().IsLengthFormatValid(command.UserName);
            if (IsInvalid()) return GetErrors();
            if (await RuleFor(p => p.Email).Email()
                .IsNotOccupiedAsync(command.Email, _uow.Users().IsExistEmailAsync))
            {
                if (!await RuleFor(p => p.Email).Email()
                .IsNotOccupiedAsync(_cache.GetSignUpEmailKey(command.Email),
                _cache.IsExistDataAsync))
                    AddError(ErrorStatus.ConfirmMail);
            }

            return GetErrors();
        }
    }
}
