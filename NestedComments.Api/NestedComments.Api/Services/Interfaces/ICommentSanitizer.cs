namespace NestedComments.Api.Services.Interfaces
{
    public interface ICommentSanitizer
    {
        bool IsContainValidTags(string input);
        string Sanitize(string input);
    }
}