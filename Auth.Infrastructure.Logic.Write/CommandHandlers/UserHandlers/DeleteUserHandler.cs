using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.UserHandlers
{
    internal class DeleteUserHandler(IUnitOfWork unitOfWork, IFileBuilder file,
        IOptionsSnapshot<FilesOptions> option) : Handler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IFileBuilder _file = file;
        private readonly FilesOptions _option = option.Value;
        public override async Task HandleAsync(DeleteUserCommand command)
        {
            var user = await _uow.Users().GetUserAsync(command.UserId);
            var tokens = await _uow.Users().GetUserTokensAsync(command.UserId);
            _uow.Remove(user);
            _uow.Remove(tokens);
            await _uow.SaveAsync();
            _file.DeleteFile(_option.AvatarsStorePath,command.UserId.ToFileName());
        }
    }
}
