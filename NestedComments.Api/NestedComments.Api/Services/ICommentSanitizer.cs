namespace NestedComments.Api.Services
{
    public interface ICommentSanitizer
    {
        bool IsContainValidTags(string input);
        string Sanitize(string input);
    }
}