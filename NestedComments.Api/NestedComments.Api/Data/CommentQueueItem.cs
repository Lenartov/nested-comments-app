using NestedComments.Api.Dtos;

namespace NestedComments.Api.Data
{
    public class CommentQueueItem
    {
        public CommentCreateDto Dto { get; set; } = default!;
        public string? FilePath { get; set; }
        public string? FileExtension { get; set; }
    }
}
