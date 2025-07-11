﻿using Microsoft.AspNetCore.Mvc;
using NestedComments.Api.Dtos;
using NestedComments.Api.Services.Interfaces;
using NestedComments.Api.Data;

namespace NestedComments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ICommentService _commentService;
        private readonly ICommentSanitizer _commentSanitizer;
        private readonly ICommentQueueService _commentQueueService;

        public CommentsController(
            IFileService fileService,
            ICommentService commentService,
            ICommentSanitizer commentSanitizer,
            ICommentQueueService commentQueueService
            )
        {
            _fileService = fileService;
            _commentService = commentService;
            _commentSanitizer = commentSanitizer;
            _commentQueueService = commentQueueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(
            int? parentId,
            string sortBy = "CreatedAt",
            string sortDir = "desc",
            int page = 1,
            int pageSize = 25)
        {
            var (items, totalCount) = await _commentService.GetCommentsAsync(parentId, sortBy, sortDir, page, pageSize);
            return Ok(new { items, totalCount });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostComment([FromForm] CommentCreateDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_commentSanitizer.IsContainValidTags(dto.Message))
                return BadRequest(new { error = "Message contains invalid content" });

            if (!_commentSanitizer.IsContainValidTags(dto.Email))
                return BadRequest(new { error = "Email contains invalid content" });

            if (!_commentSanitizer.IsContainValidTags(dto.UserName))
                return BadRequest(new { error = "Username contains invalid content" });

            if (dto.HomePage is not null && !_commentSanitizer.IsContainValidTags(dto.HomePage))
                return BadRequest(new { error = "Home page contains invalid content" });


            string? savedFilePath = null;
            string? fileExtension = null;
            if (file != null)
            {
                try
                {
                    savedFilePath = await _fileService.SaveFileAsync(file);
                    fileExtension = _fileService.GetFileExtension(file);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            _commentQueueService.Enqueue(new CommentQueueItem
            {
                Dto = dto,
                FilePath = savedFilePath,
                FileExtension = fileExtension
            });

            return Accepted(new { status = "Queued for processing" });
        }

    }
}
