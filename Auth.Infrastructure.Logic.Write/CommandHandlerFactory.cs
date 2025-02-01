using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Logic.Write
{
    internal class CommandHandlerFactory(IServiceProvider service) : ICommandHandlerFactory
    {
        private readonly IServiceProvider _service = service;

        public ICommandHandler<T> Resolve<T>() where T : Command
        {
            return _service.GetRequiredService<ICommandHandler<T>>();
        }
        public IValidator<T> ValidatorResolve<T>() where T : Command
        {
            return _service.GetRequiredService<IValidator<T>>();
        }
    }
}
