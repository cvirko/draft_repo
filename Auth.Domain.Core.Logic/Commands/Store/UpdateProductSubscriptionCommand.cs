namespace Auth.Domain.Core.Logic.Commands.Store
{
    public class UpdateProductSubscriptionCommand : Command
    {
        public UpdateProductSubscriptionCommand() { }
        public UpdateProductSubscriptionCommand(uint productId, PaymentType type, string id, bool isActive = false)
        {
            ProductId = productId;
            Type = type;
            IsActive = isActive;
            PaymentId = id;
        }
        public uint ProductId { get; set; }
        public PaymentType Type { get; set; }
        public bool IsActive { get; set; }
        public string PaymentId { get; set; }
        
    }
}
