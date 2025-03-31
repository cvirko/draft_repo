namespace Auth.Domain.Interface.Data.Read.Locks
{
    public interface IAsyncSynchronization
    {
        void Remove(string key);
        Task<SemaphoreSlim> GetLockAsync(string key);
    }
}
