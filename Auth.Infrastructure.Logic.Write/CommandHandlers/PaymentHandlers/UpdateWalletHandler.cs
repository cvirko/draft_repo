using Auth.Domain.Core.Logic.Commands.Payments;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.PaymentHandlers
{
    internal class UpdateWalletHandler(IUnitOfWork unitOfWork)
        : Handler<UpdateWalletCommand>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        public override async Task HandleAsync(UpdateWalletCommand command)
        {
            await _uow.UpdateBalanceAsync(command.ToUserId,command.Amount);
        }
    }
}
