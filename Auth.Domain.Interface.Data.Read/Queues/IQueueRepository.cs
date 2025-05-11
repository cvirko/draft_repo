namespace Auth.Domain.Interface.Data.Read.Queues
{
    public interface IQueueRepository<T>
    {
        void Add(T action);
        bool TryRemove(out T action);
        bool TryGet(out T action);
    }
}
