using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.StoreHandler
{
    internal class UpdateOrderHandler(IUnitOfWork unitOfWork)
        : Handler<UpdateOrderCommand>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        public override async Task HandleAsync(UpdateOrderCommand command)
        {
            var orderId = await GetOrAddOrderAsync(command.UserId);
            await UpdateProducts(orderId, command.ProductIds);
            await _uow.SaveAsync();
        }
        private async Task UpdateProducts(ulong orderId, Dictionary<uint, ushort> productsAmount)
        {
            var oldProductsSorted = await _uow.Store().GetOrderProductsAsync(orderId);
            int oldProductIndex = 0;
            var productIdsSorted = productsAmount.Keys.Order().ToArray();

            for (var i = 0; i < productIdsSorted.Length; i++)
            {
                AddOrUpdateProduct(orderId, ref oldProductIndex, 
                    oldProductsSorted, productIdsSorted[i], 
                    productsAmount[productIdsSorted[i]]);
            }
            WipeOffExcessOldProduct(oldProductsSorted, oldProductIndex);
        }
        private void AddOrUpdateProduct(ulong orderId, ref int oldProductIndex,
            ProductOrder[] oldProductsSorted, uint productId, ushort amount)
        {
            switch (CompareProductId(oldProductIndex, oldProductsSorted, productId))
            {
                case 0:
                    oldProductsSorted[oldProductIndex].Amount = amount;
                    oldProductIndex++;
                    break;
                case 1:
                    AddNewProduct(orderId, productId, amount);
                    break;
                default:
                    RemoveProduct(oldProductsSorted[oldProductIndex]);
                    oldProductIndex++;
                    AddOrUpdateProduct(orderId, ref oldProductIndex,
                        oldProductsSorted, productId, amount);
                    break;
            }
        }
        private short CompareProductId(int j, ProductOrder[] productsOldSorted, uint productId)
        {
            if (productsOldSorted.Length <= j) return 1;
            if (productsOldSorted[j].ProductId == productId) return 0;
            if (productsOldSorted[j].ProductId > productId) return 1;
            return -1;
        }
        private void WipeOffExcessOldProduct(ProductOrder[] productsOldSorted, int oldProductIndex)
        {
            for (int i = oldProductIndex; i < productsOldSorted.Length; i++)
            {
                RemoveProduct(productsOldSorted[i]);
            }
        }
        private void RemoveProduct(ProductOrder product)
        {
            _uow.Remove(product);
            product = null;
        }
        private void AddNewProduct(ulong orderId, uint productId, ushort amount)
        {
            _uow.Add(new ProductOrder()
            {
                ProductId = productId,
                Amount = amount,
                OrderId = orderId
            });
        }
        private async Task<ulong> GetOrAddOrderAsync(Guid userId)
        {
            var order = await _uow.Store().GetCollectingOrderAsync(userId);
            if (order is not null) return order.OrderId;

            order = new()
            {
                DateAt = DateTime.UtcNow,
                Status = OrderStatus.Collecting,
                UserId = userId,
            };
            await _uow.AddAsync(order);
            await _uow.SaveAsync();
            return order.OrderId;
        }
    }
}
