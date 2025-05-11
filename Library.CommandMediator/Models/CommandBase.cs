using Library.CommandMediator.Interfaces;

namespace Library.CommandMediator.Models
{
    public abstract class CommandBase : ICommand;
    public abstract class CommandBase<TResponse> : ICommand<TResponse>;
}
