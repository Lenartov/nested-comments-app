using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Dtos;
using NestedComments.Api.Models;

namespace NestedComments.Api.Services;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly ICommentSanitizer _sanitizer;

    public CommentService(AppDbContext context, ICommentSanitizer sanitizer)
    {
        _context = context;
        _sanitizer = sanitizer;
    }

    public async Task<Comment> CreateCommentAsync(CommentCreateDto dto, string? filePath, string fileExtension)
    {
        /*string sanitizedMessage = _sanitizer.Sanitize(dto.Message);
        string sanitizedEmail = _sanitizer.Sanitize(dto.Email);
        string sanitizedUserName = _sanitizer.Sanitize(dto.UserName);
        string? sanitizedHomePage = null;
        
        if(dto.HomePage is not null)
            sanitizedHomePage = _sanitizer.Sanitize(dto.HomePage);*/

        Comment comment = new Comment
        {
            UserName = dto.UserName,
            Email = dto.Email,
            HomePage = dto.HomePage,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = dto.ParentCommentId,
            FilePath = filePath,
            FileExtension = fileExtension
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<IEnumerable<CommentReadDto>> GetCommentsAsync(string sortBy = "CreatedAt", string sortDir = "desc", int page = 1, int pageSize = 5)
    {
        IQueryable<Comment> query = _context.Comments
            .Include(c => c.Replies)
            .Where(c => c.ParentCommentId == null);

        query = (sortBy.ToLower(), sortDir.ToLower()) switch
        {
            ("username", "asc") => query.OrderBy(c => c.UserName),
            ("username", "desc") => query.OrderByDescending(c => c.UserName),
            ("email", "asc") => query.OrderBy(c => c.Email),
            ("email", "desc") => query.OrderByDescending(c => c.Email),
            ("createdat", "asc") => query.OrderBy(c => c.CreatedAt),
            ("createdat", "desc") => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        if (page <= 0)
            page = 1;

        var comments = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return comments.Select(MapToReadDto);
    }

    public CommentReadDto MapToReadDto(Comment comment)
    {
        return new CommentReadDto
        {
            Id = comment.Id,
            UserName = comment.UserName,
            Message = comment.Message,
            CreatedAt = comment.CreatedAt,
            HomePage = comment.HomePage,
            FilePath = comment.FilePath,
            FileExtension = comment.FileExtension,
            Replies = comment.Replies?.Select(MapToReadDto).ToList() ?? new List<CommentReadDto>()
        };
    }
}
