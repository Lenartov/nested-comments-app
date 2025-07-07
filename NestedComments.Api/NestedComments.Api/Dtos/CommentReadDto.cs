using NestedComments.Api.Models;

namespace NestedComments.Api.Dtos
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? HomePage { get; set; }
        public string? FilePath { get; set; }
        public string? FileExtension { get; set; }
        public bool HasReplies { get; set; }

        public static implicit operator CommentReadDto(Comment comment)
        {
            return new CommentReadDto
            {
                Id = comment.Id,
                UserName = comment.UserName,
                Message = comment.Message,
                Email = comment.Email,
                CreatedAt = comment.CreatedAt,
                HomePage = comment.HomePage,
                FilePath = comment.FilePath,
                FileExtension = comment.FileExtension,
            };
        }
    }
}
