using Library.CommandMediator.Consts;
using Library.CommandMediator.Interfaces;
using Library.CommandMediator.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.CommandMediator.Services
{
    public class CommandDispatcher<TError, EStatus> : ICommandDispatcher
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CommandDispatcher<TError, EStatus>> _logger;
        public CommandDispatcher(
            IServiceProvider services,
            ILogger<CommandDispatcher<TError, EStatus>> logger)
        {
            _services = services;
            _logger = logger;
        }
        public async Task<TResponse> ProcessAsync<TCommand, TResponse>(TCommand command)
            where TCommand : CommandBase<TResponse>
            => await ProcessCommandAsync<CommandBase<TResponse>, TResponse>(command);
        public async Task<TResponse> ProcessAsync<TResponse>(CommandBase<TResponse> command)
            => await ProcessCommandAsync(command);
        public async Task ProcessAsync<TCommand>(TCommand command) where TCommand : CommandBase
            => await ProcessCommandAsync(command);

        private async Task<TResponse> ProcessCommandAsync<TCommand, TResponse>
            (TCommand command)
            where TCommand : CommandBase<TResponse>
        {
            var validator = GetValidator<TCommand, TResponse>();
            await validator.ValidateAsync(command);
            if (validator.IsInvalid)
                validator.Throw(command.GetType(), ExceptionConst.MESSAGE);

            var commandHandler = GetCommandHandler<TCommand, TResponse>();
            return await commandHandler.HandleAsync(command);
        }
        private async Task<TResponse> ProcessCommandAsync<TResponse>(CommandBase<TResponse> command)
        {
            var type = command.GetType();
            var validator = GetValidator<TResponse>(type);
            await validator.ValidateAsync(command);
            if (validator.IsInvalid)
                validator.Throw(command.GetType(), ExceptionConst.MESSAGE);

            var commandHandler = GetCommandHandler<TResponse>(type);
            return await commandHandler.HandleAsync(command);
        }
        private async Task ProcessCommandAsync<TCommand>(TCommand command)
           where TCommand : CommandBase
        {
            var validator = GetValidator<TCommand>();
            await validator.ValidateAsync(command);
            if (validator.IsInvalid)
                validator.Throw(command.GetType(), ExceptionConst.MESSAGE);

            var commandHandler = GetCommandHandler<TCommand>();
            await commandHandler.HandleAsync(command);
        }

        private IValidator<TCommand, TError, EStatus> GetValidator<TCommand>()
            where TCommand : CommandBase
            => (IValidator<TCommand, TError, EStatus>)TryResolve
                (typeof(IValidator<TCommand, TError, EStatus>));
        private ICommandHandler<TCommand> GetCommandHandler<TCommand>()
          where TCommand : CommandBase
            => (ICommandHandler<TCommand>)TryResolve
                (typeof(ICommandHandler<TCommand>));
        private IBaseValidator<TError, EStatus> GetValidator<TResponse>(Type type)
            => (IBaseValidator<TError, EStatus>)TryResolve(typeof(IValidator<,,,>)
                .MakeGenericType(type, typeof(TResponse), typeof(TError), typeof(EStatus)));
        private ICommandHandlerWithResponse<TResponse> GetCommandHandler<TResponse>(Type type)
            => (ICommandHandlerWithResponse<TResponse>)TryResolve(typeof(ICommandHandler<,>)
                .MakeGenericType(type, typeof(TResponse)));
        private IValidator<TCommand, TResponse, TError, EStatus> GetValidator<TCommand, TResponse>()
            where TCommand : CommandBase<TResponse>
            => (IValidator<TCommand, TResponse, TError, EStatus>)TryResolve
                (typeof(IValidator<TCommand, TResponse, TError, EStatus>));
        private ICommandHandler<TCommand, TResponse> GetCommandHandler<TCommand, TResponse>()
            where TCommand : CommandBase<TResponse>
            => (ICommandHandler<TCommand, TResponse>)TryResolve
                (typeof(ICommandHandler<TCommand, TResponse>));
        private object TryResolve(Type type)
        {
            try
            {
                return _services.GetRequiredService(type);
            }
            catch (Exception e)
            {
                throw new Exception(@$"Failed to resolve {type.FullName}. 
                    You may have declared a service in its ctor that is not available for injection.
                    See inner exception for details", e);
            }
        }
    }
}
