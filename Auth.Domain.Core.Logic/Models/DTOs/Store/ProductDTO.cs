namespace Auth.Domain.Core.Logic.Models.DTOs.Store
{
    public class ProductDTO
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
    }
}
