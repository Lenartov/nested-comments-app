using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Models;

namespace NestedComments.Api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CommentsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _context.Comments
                .Include(c => c.Replies)
                .Where(c => c.ParentCommentId == null)
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostComment([FromForm] CommentCreateDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var storedCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (string.IsNullOrEmpty(storedCaptcha) ||
                !string.Equals(storedCaptcha, dto.Captcha, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Invalid CAPTCHA" });
            }

            string? savedFilePath = null;
            if (file != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                savedFilePath = "/uploads/" + fileName;
            }


            var comment = new Comment
            {
                UserName = dto.UserName,
                Email = dto.Email,
                HomePage = dto.HomePage,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                ParentCommentId = dto.ParentCommentId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

    }
}
