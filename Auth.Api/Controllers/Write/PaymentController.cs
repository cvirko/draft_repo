using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Interface.Logic.External.Payments;
using Microsoft.Extensions.Options;

namespace Auth.Api.Controllers.Write
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PaymentController(ICommandDispatcher commandDispatcher,
        IPaymentUnitOfWork payment, IOptionsSnapshot<PaymentOptions> option) : CommandController(commandDispatcher)
    {
        private readonly IPaymentUnitOfWork _payment = payment;
        private readonly PaymentOptions _option = option.Value;

        [AllowAnonymous]
        [Route("Stripe/Webhook/{token}")]
        [HttpPost]
        public async Task<ActionResult> StripeWebHookAsync([FromRoute] string token)
        {
            if (string.IsNullOrWhiteSpace(token) ||
                _option.Stripe_WebhookEndpointToken != token)
                return Forbid();

            var result = await _payment.Stripe().ValidateWebHookAsync(Request);
            
            await _command.ProcessAsync(new ConfirmPaymentActionCommand
            {
                PaymentId = result.Id,
                TransactionId = result.TransactionId,
                Type = result.Type,
                Status = result.Status,
            });
            return Ok();
        }

        [Authorize(AuthConsts.IS_USER)]
        [Route("Pay")]
        [HttpPost]
        public async Task<ActionResult<string>> StripePayAsync([FromBody] PayForOrderCommand command)
        {
            var response = await ProcessAsync(command);
            return Ok(response.UserSecret);
        }

        [Authorize(AuthConsts.IS_USER)]
        [Route("Subscription")]
        [HttpPost]
        public async Task<ActionResult> StripeSubscriptionAsync(SubscriptionCommand command)
        {
            var response = await ProcessAsync(command);
            return Ok();
        }
    }
}
