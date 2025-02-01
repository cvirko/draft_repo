using Auth.Domain.Core.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Logic.Write
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory _commandHandlerFactory;
        private readonly ILogger<CommandDispatcher> _logger;
        public CommandDispatcher(
            ICommandHandlerFactory commandHandlerFactory,
            ILogger<CommandDispatcher> logger)
        {
            _commandHandlerFactory = commandHandlerFactory;
            _logger = logger;
        }

        public async Task ProcessAsync(Command command)
        {
            await ProcessCommandAsync((dynamic)command);
        }

        private async Task ProcessCommandAsync<TCommand>(TCommand command)
           where TCommand : Command
        {
            var validator = GetValidator<TCommand>();
            await InternalProcessValidateAsync(command, validator);

            var commandHandler = GetCommandHandler<TCommand>();
            await InternalProcessCommandAsync(command, commandHandler);
        }

        private IValidator<TCommand> GetValidator<TCommand>()
            where TCommand : Command
        {
            try
            {
                return _commandHandlerFactory.ValidatorResolve<TCommand>();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to resolve IValidator<{typeof(TCommand).Name}>. You may have declared a service in its ctor that is not available for injection. See inner exception for details", e);
            }
        }

        private ICommandHandler<TCommand> GetCommandHandler<TCommand>()
          where TCommand : Command
        {
            try
            {
                return _commandHandlerFactory.Resolve<TCommand>();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to resolve ICommandHandler<{typeof(TCommand).Name}>. You may have declared a service in its ctor that is not available for injection. See inner exception for details", e);
            }
        }

        private async Task InternalProcessValidateAsync<TCommand>(TCommand command, IValidator<TCommand> validator) where TCommand : Command
        {
            _logger.LogInformation($"Validate command {command.GetType().Name}");

            var errors = await validator.ValidateAsync(command);

            if (!validator.IsValid())
                throw new ForbiddenException(errors);

            _logger.LogInformation($"Command {command.GetType().Name} handled");
        }

        private async Task InternalProcessCommandAsync<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler) where TCommand : Command
        {
            _logger.LogInformation($"Handling command {command.GetType().Name}");

            await commandHandler.HandleAsync(command);

            _logger.LogInformation($"Command {command.GetType().Name} handled");
        }
    }
}
