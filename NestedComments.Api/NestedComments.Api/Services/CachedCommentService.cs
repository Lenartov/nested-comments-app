using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Dtos;
using NestedComments.Api.Models;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Services
{
    public class CachedCommentService : ICommentService
    {
        private readonly ICommentCacheManager _cacheManager;
        private readonly AppDbContext _context;

        public CachedCommentService(AppDbContext context, ICommentCacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        public async Task<(IEnumerable<CommentReadDto>, int)> GetCommentsAsync(int? parentId, string sortBy, string sortDir, int page, int pageSize)
        {
            return await _cacheManager.GetOrAddAsync(parentId, sortBy, sortDir, page, pageSize,
                    async () =>
                    {
                        IQueryable<Comment> query = _context.Comments
                        .Where(c => c.ParentCommentId == parentId);

                        query = (sortBy.ToLower(), sortDir.ToLower()) switch
                        {
                            ("username", "asc") => query.OrderBy(c => c.UserName),
                            ("username", "desc") => query.OrderByDescending(c => c.UserName),
                            ("email", "asc") => query.OrderBy(c => c.Email),
                            ("email", "desc") => query.OrderByDescending(c => c.Email),
                            ("createdat", "asc") => query.OrderBy(c => c.CreatedAt),
                            ("createdat", "desc") => query.OrderByDescending(c => c.CreatedAt),
                            _ => query.OrderByDescending(c => c.CreatedAt)
                        };

                        if (page <= 0)
                            page = 1;

                        int totalCount = await query.CountAsync();

                        var comments = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

                        var commentIds = comments.Select(c => c.Id).ToList();

                        var repliesCount = await _context.Comments
                            .Where(c => commentIds.Contains(c.ParentCommentId.Value))
                            .GroupBy(c => c.ParentCommentId)
                            .Select(g => new { ParentId = g.Key, Count = g.Count() })
                            .ToDictionaryAsync(x => x.ParentId!.Value, x => x.Count);

                        var commentDtos = comments.Select(comment =>
                        {
                            CommentReadDto dto = comment;
                            dto.HasReplies = repliesCount.ContainsKey(comment.Id);
                            return dto;
                        });

                        return (commentDtos, totalCount);
                    });
        }

        public async Task<Comment> CreateCommentAsync(CommentQueueItem commentData)
        {
            Comment comment = new Comment
            {
                UserName = commentData.Dto.UserName,
                Email = commentData.Dto.Email,
                HomePage = commentData.Dto.HomePage,
                Message = commentData.Dto.Message,
                CreatedAt = DateTime.UtcNow,
                ParentCommentId = commentData.Dto.ParentCommentId,
                FilePath = commentData.FilePath,
                FileExtension = commentData.FileExtension
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            _cacheManager.InvalidateGroup(comment.ParentCommentId);

            return comment;
        }

        public async Task<Comment[]> CreateCommentsAsync(CommentQueueItem[] commentsData)
        {
            Comment[] comments = new Comment[commentsData.Length];

            for (int i = 0; i < commentsData.Length; i++)
            {
                comments[i] = new Comment
                {
                    UserName = commentsData[i].Dto.UserName,
                    Email = commentsData[i].Dto.Email,
                    HomePage = commentsData[i].Dto.HomePage,
                    Message = commentsData[i].Dto.Message,
                    CreatedAt = DateTime.UtcNow,
                    ParentCommentId = commentsData[i].Dto.ParentCommentId,
                    FilePath = commentsData[i].FilePath,
                    FileExtension = commentsData[i].FileExtension
                };
            }

            _context.Comments.AddRange(comments);
            await _context.SaveChangesAsync();

            foreach (var comment in comments)
                _cacheManager.InvalidateGroup(comment.ParentCommentId);

            return comments;
        }
    }

}
