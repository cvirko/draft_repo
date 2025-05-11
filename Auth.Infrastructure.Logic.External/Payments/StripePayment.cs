using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Core.Logic.Models.Payments;
using Auth.Domain.Interface.Logic.External.Payments;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
namespace Auth.Infrastructure.Logic.External.Payments
{
    internal class StripePayment(IOptionsSnapshot<PaymentOptions> options,
        ILogger<StripePayment> logger, IStripeClient client) : IStripePayment
    {
        private readonly IStripeClient _client = client;
        private readonly ILogger<StripePayment> _logger = logger;
        private readonly PaymentOptions _options = options.Value;

        const string TRANSACTION_KEY = "transaction_id";
        const string PAYMENT_TYPE = "card";

        public async Task CreateWebhookEndpointAsync()
        {
            var options = new WebhookEndpointCreateOptions
            {
                EnabledEvents = [
                    EventTypes.InvoicePaid, 
                    EventTypes.CheckoutSessionCompleted,
                    EventTypes.CheckoutSessionExpired,
                    EventTypes.PaymentIntentSucceeded,
                    EventTypes.PaymentIntentCanceled,
                    EventTypes.PriceCreated,
                    EventTypes.PriceDeleted,
                    EventTypes.PriceUpdated,
                    EventTypes.CustomerSubscriptionCreated,
                    EventTypes.CustomerSubscriptionUpdated,
                    EventTypes.CustomerSubscriptionDeleted
                    ],
                Url = _options.Stripe_WebhookEndpointUrl,
            };
            try
            {
                var service = new WebhookEndpointService(_client);
                await service.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        public async Task<PaymentValidationResult> ValidateWebHookAsync(HttpRequest request)
        {
            try
            {
                var json = await new StreamReader(request.Body).ReadToEndAsync();
                var stripeSignature = request.Headers["Stripe-Signature"];
                var stripeEvent = EventUtility.ParseEvent(json);

                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    _options.Stripe_WebhookSecret
                );

                switch (stripeEvent.Type)
                {
                    case EventTypes.CheckoutSessionCompleted:
                        return Result(ActionStatus.Successed, stripeEvent);
                    case EventTypes.CheckoutSessionExpired:
                        return Result(ActionStatus.Expired, stripeEvent);

                    case EventTypes.PaymentIntentSucceeded:
                        return Result(ActionStatus.Successed, stripeEvent);
                    case EventTypes.PaymentIntentCanceled:
                        return Result(ActionStatus.Canceled, stripeEvent);

                    case EventTypes.PriceCreated:
                        return Result(ActionStatus.Successed, stripeEvent);
                    case EventTypes.PriceDeleted:
                        return Result(ActionStatus.Canceled, stripeEvent);
                    case EventTypes.PriceUpdated:
                        return Result(ActionStatus.Updated, stripeEvent);

                    case EventTypes.CustomerSubscriptionCreated:
                        return Result(ActionStatus.Successed, stripeEvent);
                    case EventTypes.CustomerSubscriptionUpdated:
                        return Result(ActionStatus.Updated, stripeEvent);
                    case EventTypes.InvoicePaid:
                        return Result(ActionStatus.Successed, stripeEvent);
                    case EventTypes.CustomerSubscriptionDeleted:
                        return Result(ActionStatus.Canceled, stripeEvent);
                    default: return Result(ActionStatus.None, stripeEvent);
                }
            }
            catch (StripeException ex)
            {
               throw  CatchStripeException(ex);
            }
        }
        
        public async Task<ActionStatus> GetOperationStatusAsync(string id, PaymentType type)
        {
            switch (type)
            {
                case PaymentType.Stripe_Checkout_Session:
                    var session = await new SessionService(_client).GetAsync(id);
                    return GetStatus(session.Status);
                case PaymentType.Stripe_PaymentIntent:
                    var paymentIntent = await new PaymentIntentService(_client).GetAsync(id);
                    return GetStatus(paymentIntent.Status);
                case PaymentType.Stripe_SubscriptionSchedule:
                    var subscription = await new SubscriptionService(_client).GetAsync(id);
                    return GetStatus(subscription.Status);
                case PaymentType.Stripe_ProductPrice:
                    var price = await new PriceService(_client).GetAsync(id);
                    return price.Active ? ActionStatus.Successed : ActionStatus.Canceled;
                case PaymentType.Stripe_Invoice:
                    var invoice = await new InvoiceService(_client).GetAsync(id);
                    return GetStatus(invoice.Status);
                    default: return ActionStatus.None;
            }
        }

        public async Task<(string ClientSecret, string Id)> PayAsync(ProductItem item, Guid transactionId)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = item.PriceInCents * item.Amount,
                Currency = item.Currency.ToString(),
                PaymentMethodTypes = [PAYMENT_TYPE],
                Metadata = new Dictionary<string, string> { { TRANSACTION_KEY, transactionId.ToString() } },
            };
            try
            {
                var service = new PaymentIntentService(_client);
                var paymentIntent = await service.CreateAsync(options);
                return (paymentIntent.ClientSecret, paymentIntent.Id);
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        public async Task<string> CreateCheckoutSessionAsync(ProductItem[] items, Guid transactionId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = [PAYMENT_TYPE],
                LineItems = [..items.Select(i => new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = i.Currency.ToString(),
                        UnitAmount = i.PriceInCents,
                        ProductData = new()
                        {
                            Name = i.Name
                        },
                    },
                    Quantity = i.Amount
                })],
                Mode = "payment",
                SuccessUrl = _options.Stripe_SuccessUrl,
                CancelUrl = _options.Stripe_CancelUrl,
                Metadata = new Dictionary<string, string> { { TRANSACTION_KEY, transactionId.ToString() } }
            };
            try
            {
                var service = new SessionService(_client);
                var session = await service.CreateAsync(options);
                return session.Id;
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        
        public async Task<string> CreateProductAsync(string name, Guid transactionId)
        {
            var options = new ProductCreateOptions
            {
                Name = name,
                Metadata = new Dictionary<string, string> { { TRANSACTION_KEY, transactionId.ToString() } }
            };
            try
            {
                var service = new ProductService(_client);
                var product = await service.CreateAsync(options);

                return product.Id;
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        public async Task<string> CreateSubscribtionPriceAsync(string productId, long priceInCents, PaymentCurrency currency, Guid transactionId, string interval = "month")
        {
            var options = new PriceCreateOptions
            {
                Product = productId,
                UnitAmount = priceInCents,
                Currency = currency.ToString(),
                Recurring = new PriceRecurringOptions
                {
                    Interval = interval,
                },
                Metadata = new Dictionary<string, string> { { TRANSACTION_KEY, transactionId.ToString() } }
            };
            try
            {
                var service = new PriceService(_client);
                var price = await service.CreateAsync(options);
                return price.Id;
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        
        public async Task<string> SubscribeAsync(string customerId, string paymentMethodId, string priceId, Guid transactionId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = [new SubscriptionItemOptions { Price = priceId }],
                DefaultPaymentMethod = paymentMethodId,
                Metadata = new Dictionary<string, string> { { TRANSACTION_KEY, transactionId.ToString() } }
            };
            try
            {
                var service = new SubscriptionService(_client);
                var subscription = await service.CreateAsync(options);
                return subscription.Id;
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        public async Task<string> SubscribeCancelAsync(string subscribeId, Guid transactionId)
        {
            try
            {
                var options = new SubscriptionCancelOptions
                {
                    InvoiceNow = false,
                    Prorate = true,
                    ExtraParams = { { TRANSACTION_KEY, transactionId.ToString() } }
                };
                var service = new SubscriptionService(_client);
                var subscription = await service.CancelAsync(subscribeId, options);
                return subscription.Id;
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        public async Task<string> SubscribeUpdateAsync(string subscribeId, string priceId, string subscribItemId, Guid transactionId)
        {
            try
            {
                var service = new SubscriptionService(_client);
                var subscription = await service.GetAsync(subscribeId);
                bool isExist = false;
                List<SubscriptionItemOptions> options = [..subscription.Items.Select(i =>
                {
                    if (i.Id != subscribItemId)
                        return new SubscriptionItemOptions() { Id = i.Id, Price = i.Price.Id};
                    isExist = true;
                    return new SubscriptionItemOptions() { Id = i.Id, Price = priceId };
                })];

                if (!isExist)
                    options.Add(new() { Id = subscribItemId, Price = priceId });

                var subUpdate= await service.UpdateAsync(subscribeId, new SubscriptionUpdateOptions
                {
                    Items = options,
                    Metadata = new Dictionary<string, string> { { TRANSACTION_KEY, transactionId.ToString() } }
                });
                return subUpdate.Id;
            }
            catch (StripeException ex)
            {
                throw CatchStripeException(ex);
            }
        }
        
        private Exception CatchStripeException(StripeException ex)
        {
            switch (ex?.StripeError?.Type)
            {
                case "card_error":
                case "api_connection_error":
                case "api_error":
                case "authentication_error":
                case "invalid_request_error":
                case "rate_limit_error":
                case "validation_error":
                case null:
                    return new ForbiddenException(nameof(StripePayment),"{0}: {1}",
                        ex.Message, ex);
                default:
                    return new ForbiddenException(nameof(StripePayment),"{0}: {1}{2}{3}",
                        ex.StripeError.Type, ex.StripeError.Message);
            }
        }
        private Guid GetValue(Dictionary<string, string> data, 
            string key = TRANSACTION_KEY) => Guid.Parse(data[key]);
        private PaymentValidationResult Result(ActionStatus status, Event stripeEvent)
        {
            PaymentType type = GetType(stripeEvent);
            switch (type)
            {
                case PaymentType.Stripe_Checkout_Session:
                    var session = stripeEvent.Data.Object as Session;
                    _logger.LogInformation("{0} - Id: {1}, TransactionId: {2}", stripeEvent.Type, session.Id, session.Metadata[TRANSACTION_KEY]);
                    return new PaymentValidationResult(status, type, session.Id, GetValue(session.Metadata));
                case PaymentType.Stripe_PaymentIntent:
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("{0} - Id: {1}, TransactionId: {2}", stripeEvent.Type, paymentIntent.Id, paymentIntent.Metadata[TRANSACTION_KEY]);
                    return new PaymentValidationResult(status, type, paymentIntent.Id, GetValue(paymentIntent.Metadata));
                case PaymentType.Stripe_ProductPrice:
                    var price = stripeEvent.Data.Object as Price;
                    _logger.LogInformation("{0} - Id: {1}, TransactionId: {2}", stripeEvent.Type, price.Id, price.Metadata[TRANSACTION_KEY]);
                    return new PaymentValidationResult(status, type, price.Id, GetValue(price.Metadata));
                case PaymentType.Stripe_SubscriptionSchedule:
                    var subscription = stripeEvent.Data.Object as Subscription;
                    _logger.LogInformation("{0} - Id: {1}, TransactionId: {2}", stripeEvent.Type, subscription.Id, subscription.Metadata[TRANSACTION_KEY]);
                    return new PaymentValidationResult(status, type, subscription.Id, GetValue(subscription.Metadata));
                case PaymentType.Stripe_Invoice:
                    var invoice = stripeEvent.Data.Object as Invoice;
                    _logger.LogInformation("{0} - Id: {1}, TransactionId: {2}", stripeEvent.Type, invoice.Id, invoice.Metadata[TRANSACTION_KEY]);
                    return new PaymentValidationResult(status, type, invoice.Id, GetValue(invoice.Metadata));
                default:
                    _logger.LogWarning("{0} - Not resolved", stripeEvent.Type);
                    return new PaymentValidationResult(status, type, default, default);
            }

        }
        private PaymentType GetType(Event stripeEvent)
        {
            switch (stripeEvent.Type)
            {
                case EventTypes.CheckoutSessionCompleted:
                case EventTypes.CheckoutSessionExpired:
                case EventTypes.CheckoutSessionAsyncPaymentFailed:
                case EventTypes.CheckoutSessionAsyncPaymentSucceeded:
                    return PaymentType.Stripe_Checkout_Session;
                case EventTypes.PaymentIntentSucceeded:
                case EventTypes.PaymentIntentCanceled:
                case EventTypes.PaymentIntentPaymentFailed:
                    return PaymentType.Stripe_PaymentIntent;
                case EventTypes.PriceCreated:
                case EventTypes.PriceDeleted:
                case EventTypes.PriceUpdated:
                    return PaymentType.Stripe_ProductPrice;
                case EventTypes.SubscriptionScheduleCreated:
                case EventTypes.CustomerSubscriptionDeleted:
                case EventTypes.SubscriptionScheduleUpdated:
                case EventTypes.SubscriptionScheduleCanceled:
                case EventTypes.SubscriptionScheduleAborted:
                case EventTypes.SubscriptionScheduleCompleted:
                    return PaymentType.Stripe_SubscriptionSchedule;
                case EventTypes.InvoicePaid:
                    return PaymentType.Stripe_Invoice;
                default: return PaymentType.None;
            }
        }
        private ActionStatus GetStatus(string status)
        {
            switch (status)
            {
                case "complete":
                case "succeeded":
                case "active":
                case "paid":
                    return ActionStatus.Successed;
                case "expired":
                case "incomplete_expired":
                    return ActionStatus.Expired;
                case "canceled":
                case "unpaid":
                case "paused":
                    return ActionStatus.Canceled;
                case "open":
                case "processing":
                    return ActionStatus.Pending;
                default: return ActionStatus.None;
            }
        }
    }
}
