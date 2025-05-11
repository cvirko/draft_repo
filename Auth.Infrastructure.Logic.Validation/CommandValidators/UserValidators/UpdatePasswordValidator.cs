using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Logic.External.Auth;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class UpdatePasswordValidator(IValidationRuleService validate, IUnitOfWorkRead uow,
        IPasswordHasherService password) 
        : Validator<UpdatePasswordCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly IPasswordHasherService _passwordHasher = password;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(UpdatePasswordCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor(p => p.Password).Password().IsLengthFormatValid(command.Password);
            RuleFor(p => p.NewPassword).Password().IsLengthFormatValid(command.NewPassword);
            if (IsInvalid) return GetErrors();

            var login = await _uow.Users().GetLoginByUserIdAsync(command.UserId, command.LoginId);
            if (!RuleFor().User().IsExist(login))
                return GetErrors();
            if (RuleFor(p => p.Password).Password()
                .IsMatch(login, command.Password, _passwordHasher.VerifyHashedPassword))
                RuleFor(p => p.NewPassword).User().IsNotEqual(command.Password, command.NewPassword);
            return GetErrors();
        }
    }
}
