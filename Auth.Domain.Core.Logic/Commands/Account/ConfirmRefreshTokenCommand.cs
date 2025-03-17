namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class ConfirmRefreshTokenCommand : Command
    {
        public ConfirmRefreshTokenCommand()
        { 
        }
        public ConfirmRefreshTokenCommand(string token, string userInfo)
        {
            Token = token;
            UserInfo = userInfo;
        }
        public string Token { get; set; }
        public string UserInfo { get; set; }
    }
}
