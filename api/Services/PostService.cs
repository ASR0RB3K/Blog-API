using System.Net.Mime;
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
        private readonly IMediaService _ms;

        public PostService(BlogContext context, ILogger<PostService> logger, IMediaService mediaService)
        {
            _ctx = context;
            _lg = logger;
            _ms = mediaService;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _ctx.Posts.AnyAsync(p => p.Id == id);

        public Task<Post> GetAsync(Guid id)
            => _ctx.Posts
                .Include(p => p.Comments)
                .Include(p => p.Medias)
                .FirstOrDefaultAsync(p => p.Id == id);

        public Task<List<Post>> GetAllAsync()
            => _ctx.Posts
                .Include(p => p.Medias)
                .Include(p => p.Comments)
                .ToListAsync();

        public async Task<List<Post>> GetJsonAsync()
        {
            var post = await GetAllAsync();
            var json = post.Select(p => new
            {
                Id = p.Id,
                HandlerImageId = p.HandlerImageId,
                Title = p.Title,
                Description = p.Description,
                Content = p.Content,
                Viewed = p.Viewed,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                Comment = p.Comments.Select(c => new
                {
                    Id = c.Id,
                    Author = c.Author,
                    Content = c.Content,
                    State = c.State,
                    PostId = c.PostId
                }),
                Media = p.Medias.Select(m => new
                {
                    Id = m.Id,
                    ContentType = m.ContentType
                }),
            });

            return post;
        }

        public async Task<(bool IsSuccess, Exception exception)> InsertAsync(Post post)
        {
            if(!await _ms.ExistsAsync(post.HandlerImageId))
            {
                return(false, new Exception("Not Found."));
            }

            try
            {
                await _ctx.AddAsync(post);
                await _ctx.SaveChangesAsync();

                _lg.LogInformation($"Post created in db: {post.Id}");
                return(true, null);
            }

            catch(Exception e)
            {
                _lg.LogInformation($"Post creating in db is failed: {e.Message}", e);
                return(false, e);
            }
        }

        public async Task<(bool IsSuccess, Exception exception)> UpdateAsync(Post post)
        {
            try
            {
                _ctx.Posts.Update(post);
                await _ctx.SaveChangesAsync();

                _lg.LogInformation($"Post updated in db: {post.Id}");
                return(true, null);
            }
            
            catch(Exception e)
            {
                _lg.LogInformation($"Post update is failder in db: {e.Message}", e);
                return(false, e);
            }
        }

        public async Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id)
        {
            var post = await GetAsync(id);
            if(post is default(Post))
            {
                return(false, new Exception("Not Found."));
            }

            try
            {
                _ctx.Posts.Remove(post);
                foreach(var media in post.Medias)
                {
                    _ctx.Medias.Remove(media);
                }

                foreach(var comment in post.Comments)
                {
                    _ctx.Comments.Remove(comment);
                }

                await _ctx.SaveChangesAsync();

                _lg.LogInformation("Post deleted from db.");
                return(true, null);
            }

            catch(Exception e)
            {
                _lg.LogInformation($"Post delete in db is failed: {e.Message}", e);
                return(false, e);
            }
        }
    }
}