using NestedComments.Api.Dtos;
using NestedComments.Api.Models;
using NestedComments.Api.Data;

namespace NestedComments.Api.Services.Interfaces;

public interface ICommentService
{
    Task<Comment> CreateCommentAsync(CommentQueueItem commentData);
    Task<Comment[]> CreateCommentsAsync(CommentQueueItem[] commentsData);

    Task<(IEnumerable<CommentReadDto> items, int totalCount)> GetCommentsAsync(int? parentId = null, string sortBy = "CreatedAt", string sortDir = "desc", int page = 1, int pageSize = 25);
}