using NestedComments.Api.Dtos;
using NestedComments.Api.Models;

namespace NestedComments.Api.Services;

public interface ICommentService
{
    CommentReadDto MapToReadDto(Comment comment);
    Task<Comment> CreateCommentAsync(CommentCreateDto dto, string? filePath, string fileExtension);
    Task<IEnumerable<CommentReadDto>> GetCommentsAsync(string sortBy = "CreatedAt", string sortDir = "desc", int page = 1, int pageSize = 5);
}
