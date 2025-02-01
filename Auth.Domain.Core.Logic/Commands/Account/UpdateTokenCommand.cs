namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class UpdateTokenCommand : Command
    {
        public UpdateTokenCommand() { }
        public UpdateTokenCommand(Guid tokenLoginId, Guid userId, string token, TokenType type)
        {
            Token = token;
            Type = type;
            UserId = userId;
            TokenLoginId = tokenLoginId;
        }
        public Guid TokenLoginId { get; set; }
        public TokenType Type { get; set; }
        public string Token { get; set; }
    }
}
