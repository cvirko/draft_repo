namespace Auth.Domain.Core.Logic.Commands.Store
{
    public class AddProductCommand : Command
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
    }
}
