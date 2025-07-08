using MediatR;
using NestedComments.Api.Dtos;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Events.Handlers
{
    public class SaveCacheOnGetComments : IRequestHandler<CommentsGetEvent, (IEnumerable<CommentReadDto>, int)>
    {
        private readonly ICommentCacheManager _cacheManager;

        public SaveCacheOnGetComments(ICommentCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        async Task<(IEnumerable<CommentReadDto>, int)> IRequestHandler<CommentsGetEvent, (IEnumerable<CommentReadDto>, int)>.Handle(CommentsGetEvent request, CancellationToken cancellationToken)
        {
            return await _cacheManager.GetOrAddAsync(
                request.parentId,
                request.sortBy,
                request.sortDir,
                request.page,
                request.pageSize,
                request.factory);
        }
    }
}
