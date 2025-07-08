using MediatR;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Events.Handlers
{
    public class InvalidateCacheOnSingleCommentAdded : INotificationHandler<CommentAddedEvent>
    {
        private readonly ICommentCacheManager _cacheManager;

        public InvalidateCacheOnSingleCommentAdded(ICommentCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(CommentAddedEvent notification, CancellationToken cancellationToken)
        {
            _cacheManager.InvalidateGroup(notification.comment.ParentCommentId);
            _cacheManager.InvalidateGroup(notification.grandParentId);
        }
    }
}
