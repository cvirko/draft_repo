namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class ConfirmTokenCommand : Command
    {
        public ConfirmTokenCommand()
        {
            
        }
        public ConfirmTokenCommand(string token, TokenType type, Guid tokenLoginId)
        {
            Token = token;
            TokenType = type;
            TokenLoginId = tokenLoginId;
        }
        public string Token { get; set; }
        [JsonIgnore]
        public TokenType TokenType { get; set; }
        [JsonIgnore]
        public Guid TokenLoginId { get; set; }
    }
}
