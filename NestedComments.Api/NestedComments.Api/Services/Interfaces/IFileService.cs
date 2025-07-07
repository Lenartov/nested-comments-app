namespace NestedComments.Api.Services.Interfaces
{
    public interface IFileService
    {
        string GetFileExtension(IFormFile file);
        Task<string> SaveFileAsync(IFormFile file);
    }
}
