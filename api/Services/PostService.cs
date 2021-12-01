using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Services
{
    public class PostService : IPostService
    {
        private readonly BlogContext _ctx;
        private readonly ILogger<PostService> _lg;

        public PostService(BlogContext context, ILogger<PostService> logger)
        {
            _ctx = context;
            _lg = logger;
        }

        public async Task<(bool IsSuccess, Exception exception, Post post)> CreateAsync(Post post)
        {
            try
            {
                await _ctx.Posts.AddAsync(post);
                await _ctx.SaveChangesAsync();

                _lg.LogInformation("Post created in db.");

                return(true, null, post);
            }

            catch(Exception e)
            {
                _lg.LogInformation($"Post creation in db is failed: {e.Message}");
                return(false, e, null);
            }
        }

        public Task<bool> ExistsAsync(Guid id)
            => _ctx.Posts.AnyAsync(p => p.Id == id);

        public Task<List<Post>> GetAllAsync()
            => _ctx.Posts.AsNoTracking()
            .Include(m => m.Comments)
            .Include(m => m.Medias).ToListAsync();

        public Task<List<Post>> GetIdAsync(Guid id)
            => _ctx.Posts.AsNoTracking()
                .Where(i => i.Id == id)
                .Include(m => m.Comments)
                .Include(m => m.Medias).ToListAsync();

        public Task<Post> GetAsync(Guid id)
            => _ctx.Posts.FirstOrDefaultAsync(a => a.Id == id);

        public async Task<(bool IsSuccess, Exception exception, Post post)> UpdatePostAsync(Post post)
        {
            if(!await ExistsAsync(post.Id))
            {
                _lg.LogInformation($"Deleting post from db failed: {post.Id}");

                return (false, new ArgumentException($"There is no Post with given Id: {post.Id}"), null);
            } 

            await _ctx.Posts.AnyAsync(t => t.Id == post.Id);

            post.ModifiedAt = DateTimeOffset.UtcNow;

            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(); 

            _lg.LogInformation($"Post updated: {post.Id}");
            
            return(true, null, post);  
        }

        public async Task<(bool IsSuccess, Exception Exception)> DeleteAsync(Guid id)
        {
            if(!await ExistsAsync(id))
            {
                _lg.LogInformation($"Deleting post from db failed: {id}");

                return(false, new ArgumentException($"There is no Post with given Id: {id}"));
            }

            _ctx.Posts.Remove(await GetAsync(id));
            await _ctx.SaveChangesAsync();

            _lg.LogInformation($"Post removed from db: {id}");
           
            return (true, null);
        }
    }
}