using MediatR;
using Microsoft.AspNetCore.SignalR;
using NestedComments.Api.Hubs;

namespace NestedComments.Api.Events.Handlers
{
    public class NotifyClientsOnBatchCommentAdded : INotificationHandler<CommentsBatchAddedEvent>
    {
        private readonly IHubContext<CommentHub> _hub;

        public NotifyClientsOnBatchCommentAdded(IHubContext<CommentHub> hub)
        {
            _hub = hub;
        }

        public async Task Handle(CommentsBatchAddedEvent notification, CancellationToken cancellationToken)
        {
            var affectedParentIds = notification.comments
                .Select(c => c.ParentCommentId)
                .Distinct()
                .ToList();

            await _hub.Clients.All.SendAsync("CommentsAddedBatch", affectedParentIds);
        }
    }
}
