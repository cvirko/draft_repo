namespace Auth.Domain.Core.Logic.Commands.User
{
    public class UpdateAvatarCommand : Command
    {
        public UpdateAvatarCommand() { }
        public UpdateAvatarCommand(Stream file, string type)
        {
            Avatar = file;
            ContentType = type;
        }
        public Stream Avatar { get; set; } 
        public string ContentType { get; set; } 
    }
}
