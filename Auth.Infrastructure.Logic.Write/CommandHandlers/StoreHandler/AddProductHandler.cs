using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.StoreHandler
{
    internal class AddProductHandler(IUnitOfWork uow)
        : Handler<AddProductCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        public override async Task HandleAsync(AddProductCommand command)
        {
            await _uow.AddAsync(new Product
            {
                Name = command.Name,
                Price = command.Price,
                IsActive = true,
                Type = command.Type,
            });
            await _uow.SaveAsync();
        }
    }
}
