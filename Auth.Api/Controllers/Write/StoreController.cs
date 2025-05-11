using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Api.Controllers.Write
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StoreController(ICommandDispatcher commandDispatcher) 
        : CommandController(commandDispatcher)
    {

        [Authorize(AuthConsts.IS_USER)]
        [Route("Order/Update")]
        [HttpPost]
        public async Task<ActionResult> StripePayAsync([FromBody] UpdateOrderCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }
    }
}
