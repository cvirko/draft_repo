namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class ConfirmTokenCommand : Command
    {
        public ConfirmTokenCommand()
        {
            
        }
        public ConfirmTokenCommand(string token, TokenType type)
        {
            Token = token;
            TokenType = type;
        }
        public string Token { get; set; }
        [JsonIgnore]
        public TokenType TokenType { get; set; }
    }
}
