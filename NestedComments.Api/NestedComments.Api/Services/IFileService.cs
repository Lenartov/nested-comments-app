namespace NestedComments.Api.Services
{
    public interface IFileService
    {
        string GetFileExtension(IFormFile file);
        Task<string> SaveFileAsync(IFormFile file);
    }
}
