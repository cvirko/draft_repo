using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.Cache;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class SignInValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow, ICacheRepository cache) 
        : Validator<SignInCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly ICacheRepository _cache = cache;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(SignInCommand command)
        {
            RuleFor(p => p.Password).Password().IsLengthFormatValid(command.Password);
            RuleFor(p => p.Login).Email().IsLengthFormatValid(command.Login);

            if (IsInvalid()) return GetErrors();
            var login = await _uow.Users().GetLoginByEmailAsync(command.Login);
            if (login == null )
            {
                if (await RuleFor(p => p.Login).Email().IsExistAsync(
                    _cache.GetSignUpEmailKey(command.Login),
                    _cache.IsExistDataAsync))
                    AddError(ErrorStatus.AccessDenied, "Email not Confirmed");
                return GetErrors();
            }

            RuleFor().Password().IsNotBanned(login);
            RuleFor().Password().IsHaveAttempts(login);

            if (IsInvalid()) return GetErrors();

            RuleFor().User().IsHaveAccess(login.User);
            return GetErrors();
        }
    }
}
