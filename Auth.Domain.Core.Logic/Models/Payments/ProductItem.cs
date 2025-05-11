namespace Auth.Domain.Core.Logic.Models.Payments
{
    public class ProductItem(string name, 
        decimal price, 
        PaymentCurrency currency = PaymentCurrency.usd,
        ushort amount = 1)
    {
        public string Name { get; set; } = name;
        public ushort Amount { get; set; } = amount;
        public long PriceInCents { get; set; } = Convert.ToInt64(price * 100);
        public PaymentCurrency Currency { get; set; } = currency;
    }
}
