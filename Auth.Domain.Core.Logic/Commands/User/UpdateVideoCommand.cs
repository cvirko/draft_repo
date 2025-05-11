namespace Auth.Domain.Core.Logic.Commands.User
{
    public class UpdateVideoCommand : Command
    {
        public UpdateVideoCommand() { }
        public UpdateVideoCommand(Stream file, string type)
        {
            File = file;
            ContentType = type;
        }
        public Stream File { get; set; }
        public string ContentType { get; set; }
    }
}
