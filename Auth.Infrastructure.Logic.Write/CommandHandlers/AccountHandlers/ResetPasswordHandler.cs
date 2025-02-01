using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Logic.External.Auth;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class ResetPasswordHandler(IPasswordHasherService passwordHasher, IUnitOfWork uow) 
        : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        private readonly IUnitOfWork _uow = uow;
        public async Task HandleAsync(ResetPasswordCommand command)
        {
            var login = await _uow.Users().GetLoginByUserIdAsync(command.UserId, command.LoginId);
            login.PasswordHash = _passwordHasher.HashPassword(command.UserId, command.Password);
            await _uow.SaveAsync();
        }
    }
}
