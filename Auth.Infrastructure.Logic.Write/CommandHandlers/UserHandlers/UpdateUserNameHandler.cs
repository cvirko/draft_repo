using Auth.Domain.Core.Logic.Commands.User;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.UserHandlers
{
    internal class UpdateUserNameHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserNameCommand>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        public async Task HandleAsync(UpdateUserNameCommand command)
        {
            var user = await _uow.Users().GetUserAsync(command.UserId);
            user.UserName = command.UserName;
            await _uow.SaveAsync();
        }
    }
}
