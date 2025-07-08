using NestedComments.Api.Constants;
using NestedComments.Api.Data;
using NestedComments.Api.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestedComments.Api.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationRules.UserNameMaxLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationRules.EmailMaxLength)]
        public string Email { get; set; } = null!;

        public string? HomePage { get; set; }

        [Required]
        [MaxLength(ValidationRules.MessageMaxLength)]
        public string Message { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? FilePath { get; set; }

        [MaxLength(10)]
        public string? FileExtension { get; set; }

        public int? ParentCommentId { get; set; }

        [ForeignKey("ParentCommentId")]
        public Comment? ParentComment { get; set; }

        public List<Comment> Replies { get; set; } = new();

        public static implicit operator Comment(CommentQueueItem item)
        {
            return new Comment
            {
                UserName = item.Dto.UserName,
                Email = item.Dto.Email,
                HomePage = item.Dto.HomePage,
                Message = item.Dto.Message,
                CreatedAt = DateTime.UtcNow,
                ParentCommentId = item.Dto.ParentCommentId,
                FilePath = item.FilePath,
                FileExtension = item.FileExtension
            };
        }
    }
}
