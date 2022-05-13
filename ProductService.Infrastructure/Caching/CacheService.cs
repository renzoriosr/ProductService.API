using Microsoft.Extensions.Caching.Memory;
using ProductService.Domain.Interfaces;

namespace ProductService.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private const int CacheDurationInMinutes = 5;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set(string key, object data, int? cacheTime)
        {
            using var entry = _cache.CreateEntry(key);
            entry.Value = data;
            _cache.Set(key, data, new TimeSpan(0, cacheTime ?? CacheDurationInMinutes, 0));
        }

        public bool ExistKey(string key) => _cache.Get(key) != null;

        public T Get<T>(string key) => _cache.Get<T>(key);
    }
}
