using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Domain.Interface.Logic.External.Auth;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class AddLoginInCacheValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow,
        IPasswordHasherService password, ICacheRepository cache) 
        : Validator<AddLoginInCacheCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly IPasswordHasherService _passwordHasher = password;
        private readonly ICacheRepository _cache = cache;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(AddLoginInCacheCommand command)
        {
            RuleFor(p => p.Email).Email().IsLengthFormatValid(command.Email);
            RuleFor(p => p.Password).Password().IsLengthFormatValid(command.Password);

            if (IsInvalid()) return GetErrors();
            if (!await RuleFor(p => p.Email).Email()
                .IsNotOccupiedAsync(command.Email, _uow.Users().IsExistEmailAsync))
                return GetErrors();
            if (!await RuleFor(p => p.Email).Email()
                .IsNotOccupiedAsync(_cache.GetSignUpEmailKey(command.Email),
                _cache.IsExistDataAsync))
                return GetErrors();

            var login = await _uow.Users().GetLoginByUserIdAsync(command.UserId, command.LoginId);
            if (!RuleFor().User().IsExist(login))
                return GetErrors();
            RuleFor(p => p.Password).Password()
                .IsMatch(login, command.Password, _passwordHasher.VerifyHashedPassword);

            return GetErrors();
        }
    }
}
