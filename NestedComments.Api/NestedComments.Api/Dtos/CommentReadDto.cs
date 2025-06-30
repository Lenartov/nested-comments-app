namespace NestedComments.Api.Dtos
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? HomePage { get; set; }
        public string? FilePath { get; set; }
        public string? FileExtension { get; set; }

        public List<CommentReadDto> Replies { get; set; } = new();
    }
}
