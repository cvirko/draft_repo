using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Logic.External.Auth;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.UserHandlers
{
    internal class UpdatePasswordHandler(IUnitOfWork unitOfWork, IPasswordHasherService passwordHasher)
        : ICommandHandler<UpdatePasswordCommand>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        public async Task HandleAsync(UpdatePasswordCommand command)
        {
            var login = await _uow.Users().GetLoginByUserIdAsync(command.UserId, command.LoginId);
            login.PasswordHash = _passwordHasher.HashPassword(command.UserId, command.NewPassword);
            await _uow.SaveAsync();
        }
    }
}
