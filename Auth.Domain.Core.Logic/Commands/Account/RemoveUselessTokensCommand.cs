namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class RemoveUselessTokensCommand : Command
    {
        public CancellationToken Token { get; set; }
    }
}
