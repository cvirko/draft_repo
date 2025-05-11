using Auth.Domain.Interface.Data.Read.Queues;
using System.Collections.Concurrent;

namespace Auth.Infrastructure.Data.Queues
{
    internal class QueueRepository<T> : IQueueRepository<T>
    {
        private ConcurrentQueue<T> concurrentQueue = new();

        public void Add(T action) => concurrentQueue.Enqueue(action);
        public bool TryRemove(out T action) => concurrentQueue.TryDequeue(out action);
        public bool TryGet(out T action) => concurrentQueue.TryPeek(out action);
    }
}
