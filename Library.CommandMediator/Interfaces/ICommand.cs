namespace Library.CommandMediator.Interfaces
{
    public interface ICommand;
    public interface ICommand<out TResponse>;
}
