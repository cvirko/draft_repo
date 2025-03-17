namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class UpdateTokenCommand : Command
    {
        public UpdateTokenCommand() { }
        public UpdateTokenCommand(Guid userId, string token, TokenType type, string userLoginInfo)
        {
            Token = token;
            Type = type;
            UserId = userId;
            UserLoginInfo = userLoginInfo;
        }
        public TokenType Type { get; set; }
        public string Token { get; set; }
        public string UserLoginInfo { get; set; }
    }
}
