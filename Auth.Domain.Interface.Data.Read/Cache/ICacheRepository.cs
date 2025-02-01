namespace Auth.Domain.Interface.Data.Read.Cache
{
    public interface ICacheRepository
    {
        float AbsoluteTimeStorageInMinutes { get; }
        float SlidingTimeStorageInMinutes { get; }
        Task<T> GetDataAsync<T>(string key);
        Task<T> GetOrUpdateValuesAsync<T>(string key, Func<Task<T>> operation, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null);
        Task<T> GetOrUpdateValuesAsync<T, V>(string key, V param, Func<V, Task<T>> operation, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null);
        Task<T> GetOrUpdateValuesAsync<T, V, M>(string key, V param, Func<V, Task<M>> operation, Func<M, T> mapper, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null);
        Task SetDataAsync<T>(string key, T value, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null);
        Task RemoveDataAsync(string key);
        string GetSignUpEmailKey(string email);
        Task<bool> IsExistDataAsync(string key);
    }
}
