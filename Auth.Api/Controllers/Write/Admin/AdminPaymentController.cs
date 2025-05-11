using Auth.Domain.Core.Logic.Commands.Payments;

namespace Auth.Api.Controllers.Write.Admin
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AdminPaymentController(ICommandDispatcher commandDispatcher)
        : CommandController(commandDispatcher)
    {

        [Authorize(AuthConsts.IS_ADMIN)]
        [Route("Stripe/Subscription/Create/{productId}")]
        [HttpPost]
        public async Task<ActionResult> StripeProductAsync([FromRoute] uint productId)
        {
            await ProcessAsync(new CreateProductSubscriptionCommand
            {
                Type = PaymentType.Stripe_ProductPrice,
                Currency = PaymentCurrency.usd,
                ProductId = productId
            });
            return Ok();
        }
    }
}
