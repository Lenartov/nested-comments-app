using NestedComments.Api.Services.Interfaces;
using System.Collections.Concurrent;
using NestedComments.Api.Data;

namespace NestedComments.Api.Services
{
    public class CommentQueueService : ICommentQueueService
    {
        private readonly ConcurrentQueue<CommentQueueItem> _queue = new();

        public void Enqueue(CommentQueueItem item) => _queue.Enqueue(item);

        public bool TryDequeue(out CommentQueueItem? item) => _queue.TryDequeue(out item);
    }
}
