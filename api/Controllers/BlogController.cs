using System.Linq;
using System.Threading.Tasks;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using Microsoft.Extensions.Logging;
using api.Data;
using api.Entity;
using System;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IPostService _ps;
        private readonly IMediaService _ms;
        private readonly BlogContext _ctx;

        public BlogController(IPostService postservice, BlogContext context, IMediaService mediaService)
        {
            _ps = postservice;
            _ms = mediaService;
            _ctx = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(NewPost post)
        {
            foreach(var media in post.MediaId)
            {
                if (await _ms.ExistsAsync(post.MediaId))
                {
                    return BadRequest($"MediasId with given ID: {post.MediaId} not found.");
                }
            }
            
            var medias = await _ms.GetAllAsync(post.MediaId);
            var entity = new Post(
                headerImageId: post.HeaderImageId,
                title: post.Title,
                description: post.Description,
                content: post.Content,
                comments: null,
                medias: medias);

            var result = await _ps.InsertAsync(entity);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var posts = await _ps.GetAllAsync();
            var json = posts.Select(p => new
            {
                Id = p.Id,
                HeaderImageId = p.HeaderImageId,
                Title = p.Title,
                Description = p.Description,
                Content = p.Content,
                Viewed = p.Viewed,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                Comments = p.Comments.Select(c => new
                {
                    Id = c.Id,
                    Author = c.Author,
                    Content = c.Content,
                    State = c.State,
                    PostId = c.PostId
                }),
                Medias = p.Medias.Select(m => new
                {
                    Id = m.Id,
                    ContentType = m.ContentType
                }),
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var post = await _ps.GetAsync(id);
            return Ok(new
            {
                Id = post.Id,
                HeaderImageId = post.HeaderImageId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Viewed = post.Viewed,
                CreatedAt = post.CreatedAt,
                ModifiedAt = post.ModifiedAt,
                Comments = post.Comments.Select(c => new
                {
                    Id = c.Id,
                    Author = c.Author,
                    Content = c.Content,
                    State = c.State,
                    PostId = c.PostId
                }),
                Medias = post.Medias.Select(m => new
                {
                    Id = m.Id,
                    ContentType = m.ContentType,
                })
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, NewPost post)
        {
            if (!await _ps.ExistsAsync(id))
            {
                return BadRequest($"Not found.");
            }
            var medias = await _ms.GetAllAsync(post.MediaId);
            var entity = new Post(
                headerImageId: post.HeaderImageId,
                title: post.Title,
                description: post.Description,
                content: post.Content,
                comments: null,
                medias: medias);

            entity.Id = id;

            var result = await _ps.UpdateAsync(entity);

            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
            => Ok(await _ps.DeleteAsync(id));
    }
}