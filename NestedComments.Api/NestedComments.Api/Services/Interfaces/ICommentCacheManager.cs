using NestedComments.Api.Dtos;

namespace NestedComments.Api.Services.Interfaces
{
    public interface ICommentCacheManager
    {
        Task<(IEnumerable<CommentReadDto>, int)> GetOrAddAsync(
            int? parentId,
            string sortBy,
            string sortDir,
            int page,
            int pageSize,
            Func<Task<(IEnumerable<CommentReadDto>, int)>> factory);

        void InvalidateGroup(int? parentId);
    }

}
