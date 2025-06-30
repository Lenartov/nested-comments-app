using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestedComments.Api.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Url]
        public string? HomePage { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? FilePath { get; set; }

        [MaxLength(10)]
        public string? FileExtension { get; set; }

        public int? ParentCommentId { get; set; }

        [ForeignKey("ParentCommentId")]
        public Comment? ParentComment { get; set; }

        public List<Comment> Replies { get; set; } = new();
    }
}
