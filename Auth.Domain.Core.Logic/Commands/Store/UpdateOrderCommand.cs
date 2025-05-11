namespace Auth.Domain.Core.Logic.Commands.Store
{
    public class UpdateOrderCommand : Command
    {
        public Dictionary<uint, ushort> ProductIds { get; set; }
    }
}
