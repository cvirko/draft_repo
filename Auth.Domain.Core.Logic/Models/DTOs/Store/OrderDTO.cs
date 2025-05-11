namespace Auth.Domain.Core.Logic.Models.DTOs.Store
{
    public class OrderDTO
    {
        public PaymentCurrency Currency { get; set; }
        public IEnumerable<ProductOrderDTO> Products { get; set; }
    }
}
