namespace Auth.Domain.Core.Data.DBEntity.Store
{
    public class ProductOrder : TEntity
    {
        public ulong OrderId { get; set; }
        public Order Order { get; set; }
        public uint ProductId { get; set; }
        public Product Product { get; set; }
 
        public ushort Amount { get; set; }
        public decimal Price { get; set; }
    }
}
