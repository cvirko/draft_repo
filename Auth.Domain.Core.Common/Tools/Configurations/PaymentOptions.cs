namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class PaymentOptions : Options
    {
        public string Stripe_SecretKey { get; set; }
        public string Stripe_PublishableKey { get; set; }
        public string Stripe_WebhookSecret { get; set; }
        public string Stripe_SuccessUrl { get; set; }
        public string Stripe_CancelUrl { get; set; }
        public string Stripe_WebhookEndpointUrl { get; set; }
        public string Stripe_WebhookEndpointToken { get; set; }
        public int Stripe_MaxNetworkRetries { get; set; }
        public float FrequencyOfTransactionApprovalChecksInMinutes { get; set; }
        public float ExpirePendingInMinutes { get; set; }
    }
}
