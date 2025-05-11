using Auth.Domain.Core.Logic.Models.DTOs.Store;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.StoreBuilder;

namespace Auth.Api.Controllers.Read
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StoreController(IStoreBuilder store) : ControllerBase
    {
        private readonly IStoreBuilder _store = store;

        [Authorize(AuthConsts.IS_USER)]
        [Route("Products")]
        [HttpGet]
        public async Task<ActionResult<ProductDTO[]>> GetProducts()
        {
            var products = await _store.GetProductsAsync();
            return Ok(products);
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Product/Subscripes/{productId}")]
        [HttpGet]
        public async Task<ActionResult<ProductSubscriptionDTO[]>> GetProducts(uint productId)
        {
            var products = await _store.GetSubscriptionsAsync(productId);
            return Ok(products);
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Order")]
        [HttpGet]
        public async Task<ActionResult<OrderDTO[]>> GetOrder()
        {
            var products = await _store.GetOrderAsync(User.GetUserId());
            return Ok(products);
        }
    }
}
