using Microsoft.Extensions.Caching.Memory;
using NestedComments.Api.Dtos;
using NestedComments.Api.Models;
using NestedComments.Api.Services.Interfaces;
using System.Collections.Concurrent;
using System.Globalization;

namespace NestedComments.Api.Services
{
    public class CommentCacheManager : ICommentCacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, HashSet<string>> _groupedKeys = new();

        public CommentCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        private string BuildCacheKey(int? parentId, string sortBy, string sortDir, int page, int pageSize)
            => $"comments:{parentId?.ToString() ?? "null"}:{sortBy}:{sortDir}:{page}:{pageSize}";

        private string BuildGroupKey(int? parentId)
            => $"comments:{parentId?.ToString() ?? "null"}";

        public async Task<(IEnumerable<CommentReadDto>, int)> GetOrAddAsync(int? parentId, string sortBy, string sortDir, int page, int pageSize,
            Func<Task<(IEnumerable<CommentReadDto>, int)>> factory)
        {
            var cacheKey = BuildCacheKey(parentId, sortBy, sortDir, page, pageSize);
            var groupKey = BuildGroupKey(parentId);


            if (_cache.TryGetValue(cacheKey, out (IEnumerable<CommentReadDto>, int) cached))
            {
                return cached;
            }

            var result = await factory();

            _cache.Set(cacheKey, result);

            var keys = _groupedKeys.GetOrAdd(groupKey, _ => new HashSet<string>());
            lock (keys)
            {
                keys.Add(cacheKey);
            }

            return result;
        }

        public void InvalidateGroup(int? parentId)
        {
            var groupKey = BuildGroupKey(parentId);
            if (_groupedKeys.TryGetValue(groupKey, out var keys))
            {
                lock (keys)
                {
                    foreach (var key in keys)
                    {
                        _cache.Remove(key);
                    }
                    keys.Clear();
                }
            }
        }
    }

}
