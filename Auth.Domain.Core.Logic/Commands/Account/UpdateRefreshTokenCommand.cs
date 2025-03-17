namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class UpdateRefreshTokenCommand : Command
    {
        public UpdateRefreshTokenCommand() { }
        public UpdateRefreshTokenCommand(string userLoginInfo, Guid userId, string token)
        {
            Token = token;
            UserId = userId;
            UserLoginInfo = userLoginInfo;
        }
        public string UserLoginInfo { get; set; }
        public string Token { get; set; }
    }
}
