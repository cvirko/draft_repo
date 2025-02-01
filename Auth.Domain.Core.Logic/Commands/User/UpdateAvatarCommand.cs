namespace Auth.Domain.Core.Logic.Commands.User
{
    public class UpdateAvatarCommand(Stream file, string type) : Command
    {
        public Stream Avatar { get; set; } = file;
        public string ContentType { get; set; } = type;
    }
}
