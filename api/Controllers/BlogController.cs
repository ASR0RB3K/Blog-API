using System.Linq;
using System.Threading.Tasks;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using Microsoft.Extensions.Logging;
using api.Data;
using api.Entity;

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

        // [HttpPost]
        // public async Task<IActionResult> PostAsync(NewPost post)
        // {
        //     foreach(var media in post.MediaId)
        //     {
        //         if(await _ms.ExistsAsync(media))
        //         {
        //             return BadRequest($"Media: {post.MediaId} not found.");
        //         }
        //         var medias = await _ms.GetAllAsync(post.MediaId);
        //         var entity = new Post(
        //             handlerImageId: post.HandlerImageId,
        //             title: post.Title,
        //             description: post.Description,
        //             content: post.Content,
        //             comments: null,
        //             medias: medias);
        //     }
        // }
    }
}