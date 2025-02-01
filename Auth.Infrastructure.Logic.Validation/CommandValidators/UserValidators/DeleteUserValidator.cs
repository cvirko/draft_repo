using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Logic.External.Auth;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class DeleteUserValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow,
        IPasswordHasherService password) 
        : Validator<DeleteUserCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly IPasswordHasherService _passwordHasher = password;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(DeleteUserCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());
            RuleFor(p => p.Password).Password().IsLengthFormatValid(command.Password);

            if (IsInvalid()) return GetErrors();

            var login = await _uow.Users().GetLoginByUserIdAsync(command.UserId, command.LoginId);
            if (!RuleFor().User().IsExist(login))
                return GetErrors();
            RuleFor(p => p.Password).Password()
                .IsMatch(login, command.Password, _passwordHasher.VerifyHashedPassword);

            return GetErrors();
        }
    }
}
