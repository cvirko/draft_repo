using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Interface.Data.Read.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Auth.Infrastructure.Data.Cache
{
    internal class CacheRepository(IDistributedCache cache, IOptionsSnapshot<CacheOptions> options) : ICacheRepository
    {
        private IDistributedCache _cache = cache;
        private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new();
        private readonly CacheOptions _options = options.Value;

        public float AbsoluteTimeStorageInMinutes => _options.AbsoluteTimeStorageInMinutes;
        public float SlidingTimeStorageInMinutes => _options.SlidingTimeStorageInMinutes;
        public string GetSignUpEmailKey(string email) => string.Format("{0}_SignUpEmail", email);
        public string GetConnectionKey(Guid userId) => string.Format("{0}_Connection", userId);
        public string GetConnectionKey(string connectionId) => string.Format("{0}_Connection", connectionId);
        public async Task<bool> IsExistDataAsync(string key)
        {
            EmtyCheck(key);
            var str = await _cache.GetStringAsync(key);
            return str != null;
        }
        public async Task<T> GetDataAsync<T>(string key)
        {
            EmtyCheck(key);

            var str = await _cache.GetStringAsync(key);

            if (str == null) return default;
            return JsonConvert.DeserializeObject<T>(str);
        }
        public async Task<T> GetOrUpdateValuesAsync<T>(string key, Func<Task<T>> operation, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            var result = await GetDataAsync<T>(key);
            if (result == null)
            {
                var mylock = await GetLockAsync(key);
                try
                {
                    result = await GetDataAsync<T>(key);
                    if (result == null)
                    {
                        result = await operation();
                        if (result != null)
                            await SetAsync(key, result, absoluteTime, slidingTime);
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return result;
        }
        public async Task<T> GetOrUpdateValuesAsync<T, V>(string key, V param, Func<V, Task<T>> operation, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            var result = await GetDataAsync<T>(key);
            if (result == null)
            {
                var mylock = await GetLockAsync(key);
                try
                {
                    result = await GetDataAsync<T>(key);
                    if (result == null)
                    {
                        result = await operation(param);
                        if (result != null)
                            await SetAsync(key, result, absoluteTime, slidingTime);
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return result;
        }

        public async Task<T> GetOrUpdateValuesAsync<T, V, M>(string key, V param, Func<V, Task<M>> operation, Func<M, T> mapper, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            var result = await GetDataAsync<T>(key);
            if (result == null)
            {
                var mylock = await GetLockAsync(key);
                try
                {
                    result = await GetDataAsync<T>(key);
                    if (result == null)
                    {
                        result = mapper(await operation(param));
                        if (result != null)
                            await SetAsync(key, result, absoluteTime, slidingTime);
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return result;
        }

        public async Task SetDataAsync<T>(string key, T value, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            var mylock = await GetLockAsync(key);
            try
            {
                await SetAsync(key, value, absoluteTime, slidingTime);
            }
            finally
            {
                mylock.Release();
            }
        }

        public async Task RemoveDataAsync(string key)
        {
            EmtyCheck(key);
            await _cache.RemoveAsync(key);
        }
        private async Task SetAsync<T>(string key, T value, TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            EmtyCheck(key);
            if (value == null)
                throw new ArgumentNullException(typeof(T).Name);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value),
                     GetOptions(absoluteTime, slidingTime));
        }
        private DistributedCacheEntryOptions GetOptions(TimeSpan? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            var option = new DistributedCacheEntryOptions();
            if (absoluteTime != null)
                option.SetAbsoluteExpiration(absoluteTime.Value);
            if (slidingTime != null)
                option.SetAbsoluteExpiration(slidingTime.Value);
            return option;
        }
        private async Task<SemaphoreSlim> GetLockAsync(string key)
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
            await mylock.WaitAsync();
            return mylock;
        }
        private void EmtyCheck(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
        }
    }
}
