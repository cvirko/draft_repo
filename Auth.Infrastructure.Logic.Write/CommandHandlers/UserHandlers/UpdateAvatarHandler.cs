using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Logic.External.Files;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.UserHandlers
{
    internal class UpdateAvatarHandler(IImageService image, IOptionsSnapshot<FilesOptions> option,
        IFileBuilder file) : ICommandHandler<UpdateAvatarCommand>
    {
        private readonly IImageService _image = image;
        private readonly FilesOptions _options = option.Value;
        private readonly IFileBuilder _file = file;
        public Task HandleAsync(UpdateAvatarCommand command)
        {
            var avatar = _image.ReSizePng(command.Avatar);
            _file.ReWriteFile(_options.AvatarsStorePath, command.UserId.ToFileName(), avatar);
            return Task.CompletedTask;
        }
    }
}
