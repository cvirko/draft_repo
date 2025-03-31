namespace Auth.Client.ConsoleApp.Interfaces.Actions
{
    internal interface IActionService : IDisposable
    {
        Task AddActionsAsync(Func<Task> returnActionasync);
    }
}
