using MediatR;
using NestedComments.Api.Models;

namespace NestedComments.Api.Events
{
    public record CommentsBatchAddedEvent(Comment[] comments, List<int?> grandParentsId) : INotification;

}
