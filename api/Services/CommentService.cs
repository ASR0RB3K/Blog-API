using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Services
{
    public class CommentService : ICommentService
    {
        private readonly BlogContext _ctx;
        private readonly ILogger<CommentService> _lg;
        private readonly IPostService _ps;

        public CommentService(BlogContext context, ILogger<CommentService> logger, IPostService postService)
        {
            _ctx = context;
            _lg = logger;
            _ps = postService;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _ctx.Comments.AnyAsync(c => c.Id == id);

        public Task<Comment> GetAsync(Guid id)
            => _ctx.Comments.FirstOrDefaultAsync(c => c.Id == id);

        public Task<List<Comment>> GetAllAsync()
            => _ctx.Comments.ToListAsync();

        public async Task<(bool IsSuccess, Exception exception)> InsertAsync(Comment comment)
        {
            try
            {
                await _ctx.Comments.AddAsync(comment);
                await _ctx.SaveChangesAsync();

                _lg.LogInformation("Comment added to db.");
                return(true, null);
            }

            catch(Exception e)
            {
                _lg.LogInformation($"Comment adding is failder in db: {e.Message}");
                return(false, e);
            }
        }

        public async Task<(bool IsSuccess, Exception exception)> UpdateAsync(Comment comment)
        {
            if(!await _ps.ExistsAsync(comment.PostId))
            {
                return(false, new Exception("Not Found"));
            }

            try
            {
                _ctx.Comments.Update(comment);
                await _ctx.SaveChangesAsync();

                _lg.LogInformation($"Comment updated in db: {comment.Id}");
                return(true, null);
            }

            catch(Exception e)
            {
                _lg.LogInformation($"Comment update is failed in db: {e.Message}");
                return(false, e);
            }
        }

        public async Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id)
        {
            var ncomment = await GetAsync(id);
            if(ncomment is default(Comment))
            {
                return(false, new Exception("Not Found."));
            }

            try
            {
                _ctx.Comments.Remove(ncomment);
                await _ctx.SaveChangesAsync();

                _lg.LogInformation($"Comment deleted from db: {ncomment.Id}");
                return(true, null);
            }

            catch(Exception e)
            {
                _lg.LogInformation($"Comment delete in db is failed: {e.Message}");
                return(false, e);
            }
        }
    }
}