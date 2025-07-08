using MediatR;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Events.Handlers
{
    public class InvalidateCacheOnBatchCommentAdded : INotificationHandler<CommentsBatchAddedEvent>
    {
        private readonly ICommentCacheManager _cacheManager;

        public InvalidateCacheOnBatchCommentAdded(ICommentCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(CommentsBatchAddedEvent notification, CancellationToken cancellationToken)
        {
            foreach (var comment in notification.comments)
            {
                _cacheManager.InvalidateGroup(comment.ParentCommentId);
            }

            foreach (var parentId in notification.grandParentsId)
            {
                _cacheManager.InvalidateGroup(parentId);
            }
        }
    }
}
