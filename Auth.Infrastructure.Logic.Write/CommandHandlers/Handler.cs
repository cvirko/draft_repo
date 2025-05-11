using Library.CommandMediator.Services;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers
{
    internal abstract class Handler<TCommand> 
        : BaseHandler<TCommand>
        where TCommand : Command;
    internal abstract class Handler<TCommand, TResponse> 
        : BaseHandler<TCommand, TResponse>
        where TCommand : Command<TResponse>;
}
