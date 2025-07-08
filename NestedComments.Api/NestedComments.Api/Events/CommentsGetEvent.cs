using MediatR;
using NestedComments.Api.Dtos;

namespace NestedComments.Api.Events
{
    public record CommentsGetEvent(int? parentId, string sortBy, string sortDir, int page, int pageSize, Func<Task<(IEnumerable<CommentReadDto>, int)>> factory) : IRequest<(IEnumerable<CommentReadDto>, int)>;
}
