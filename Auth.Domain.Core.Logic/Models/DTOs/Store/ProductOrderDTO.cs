namespace Auth.Domain.Core.Logic.Models.DTOs.Store
{
    public class ProductOrderDTO
    {
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public ushort Amount { get; set; }
        public decimal Price { get; set; }
    }
}
