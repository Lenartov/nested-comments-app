using MediatR;
using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Dtos;
using NestedComments.Api.Events;
using NestedComments.Api.Models;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Services
{
    public class CachedCommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public CachedCommentService(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<(IEnumerable<CommentReadDto>, int)> GetCommentsAsync(int? parentId, string sortBy, string sortDir, int page, int pageSize)
        {
            return await _mediator.Send(new CommentsGetEvent(parentId, sortBy, sortDir, page, pageSize,
                    async () => await GetCommentsRequest(parentId, sortBy, sortDir, page, pageSize)));
        }

        public async Task<Comment> CreateCommentAsync(CommentQueueItem commentData)
        {
            Comment comment = commentData;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            int? grandParentId = await GetParentId(comment);
            await _mediator.Publish(new CommentAddedEvent(comment, grandParentId));

            return comment;
        }

        public async Task<Comment[]> CreateCommentsAsync(CommentQueueItem[] commentsData)
        {
            Comment[] comments = new Comment[commentsData.Length];
            for (int i = 0; i < commentsData.Length; i++)
                comments[i] = commentsData[i];
            
            _context.Comments.AddRange(comments);
            await _context.SaveChangesAsync();

            var parentIds = commentsData
                .Select(c => c.Dto.ParentCommentId)
                .Where(id => id != null)
                .Cast<int>()
                .Distinct()
                .ToList();
            List<int?> grandParentIds = await GetParentsIdList(parentIds);
            await _mediator.Publish(new CommentsBatchAddedEvent(comments, grandParentIds));

            return comments;
        }

        private async Task<(IEnumerable<CommentReadDto>, int)> GetCommentsRequest(int? parentId, string sortBy, string sortDir, int page, int pageSize)
        {
            IQueryable<Comment> query = _context.Comments.Where(c => c.ParentCommentId == parentId);

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
        }

        private async Task<List<int?>> GetParentsIdList(List<int> parentIds)
        {
            return await _context.Comments
                .Where(c => parentIds.Contains(c.Id))
                .Select(c => c.ParentCommentId)
                .Distinct()
                .ToListAsync();
        }

        private async Task<int?> GetParentId(Comment comment)
        {
            return await _context.Comments
                .Where(c => c.Id == comment.ParentCommentId)
                .Select(c => c.ParentCommentId)
                .FirstOrDefaultAsync();
        }
    }

}
