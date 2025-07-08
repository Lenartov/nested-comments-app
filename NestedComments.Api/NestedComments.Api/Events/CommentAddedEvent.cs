using MediatR;
using NestedComments.Api.Models;

namespace NestedComments.Api.Events
{
    public record CommentAddedEvent(Comment comment, int? grandParentId) : INotification;

}
