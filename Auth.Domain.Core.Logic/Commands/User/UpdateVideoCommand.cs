namespace Auth.Domain.Core.Logic.Commands.User
{
    public class UpdateVideoCommand(Stream file, string type) : Command
    {
        public Stream File { get; set; } = file;
        public string ContentType { get; set; } = type;
    }
}
