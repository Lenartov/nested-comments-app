using NestedComments.Api.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace NestedComments.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetFileExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName).ToLowerInvariant();
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var allowedImageExtensions = new[] { ".jpg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (allowedImageExtensions.Contains(fileExtension))
            {
                using var image = Image.Load(file.OpenReadStream());

                if (image.Width > 320 || image.Height > 240)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(320, 240)
                    }));
                }

                await using var stream = new FileStream(filePath, FileMode.Create);
                await image.SaveAsync(stream, image.DetectEncoder(fileExtension));

                return "/uploads/" + fileName;
            }
            else if (fileExtension == ".txt")
            {
                if (file.Length > 100 * 1024)
                    throw new InvalidOperationException("Text file exceeds 100 KB.");

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return "/uploads/" + fileName;
            }
            else
            {
                throw new InvalidOperationException("Invalid file format.");
            }
        }
    }

}
