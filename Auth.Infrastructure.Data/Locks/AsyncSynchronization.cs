using Auth.Domain.Interface.Data.Read.Locks;
using System.Collections.Concurrent;

namespace Auth.Infrastructure.Data.Locks
{
    internal class AsyncSynchronization: IAsyncSynchronization
    {
        private static ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public void Remove(string key)
        {
            if (_locks.TryRemove(key, out var _lock))
                _lock.Dispose();
        }
        public async Task<SemaphoreSlim> GetLockAsync(string key)
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
            await mylock.WaitAsync();
            return mylock;
        }
    }
}
