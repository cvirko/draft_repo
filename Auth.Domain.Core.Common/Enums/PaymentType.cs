namespace Auth.Domain.Core.Common.Enums
{
    public enum PaymentType
    {
        None = 0,
        Stripe_Checkout_Session,// creates a payment link
        Stripe_PaymentIntent,// with confirmation
        Stripe_Charge, // Single payments do not work in Europe
        Stripe_ProductPrice,
        Stripe_SubscriptionSchedule,
        Stripe_Invoice
    }
}
