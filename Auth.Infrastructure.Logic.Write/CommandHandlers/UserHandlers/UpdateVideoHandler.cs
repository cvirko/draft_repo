using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.UserHandlers
{
    internal class UpdateVideoHandler(IOptionsSnapshot<FilesOptions> option,
        IFileBuilder file) : ICommandHandler<UpdateVideoCommand>
    {
        private readonly FilesOptions _options = option.Value;
        private readonly IFileBuilder _file = file;
        public async Task HandleAsync(UpdateVideoCommand command)
        {
            var extension = FileExtension.GetExtension(command.ContentType);
            await _file.ReWriteFileAsync(_options.VideoStorePath, command.UserId.ToFileName(extension), command.File);
        }
    }
}
