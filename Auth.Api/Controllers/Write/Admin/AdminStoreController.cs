using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Api.Controllers.Write.Admin
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AdminStoreController(ICommandDispatcher dispatcher) : CommandController(dispatcher)
    {

        [Authorize(AuthConsts.IS_ADMIN)]
        [Route("Product/Add")]
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }
        [Authorize(AuthConsts.IS_ADMIN)]
        [Route("Product/Subscription/Update")]
        [HttpPost]
        public async Task<ActionResult> UpdateProductSubscription([FromBody] UpdateProductSubscriptionCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }
        [Authorize(AuthConsts.IS_ADMIN)]
        [Route("Wallet/Update")]
        [HttpPost]
        public async Task<ActionResult> UpdateWallet([FromBody] UpdateWalletCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }
    }
}
