using NestedComments.Api.Dtos;
using NestedComments.Api.Data;

namespace NestedComments.Api.Services.Interfaces
{
    public interface ICommentQueueService
    {
        void Enqueue(CommentQueueItem item);
        bool TryDequeue(out CommentQueueItem? item);
    }
}
