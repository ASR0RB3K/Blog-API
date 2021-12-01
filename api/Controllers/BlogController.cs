using System.Linq;
using System.Threading.Tasks;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using Microsoft.Extensions.Logging;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly PostService _ps;
        private readonly ILogger<BlogController> _lg;
        private readonly IMediaService _ms;
        private readonly ICommentService _cs;

        public BlogController(PostService postservice, ILogger<BlogController> logger, IMediaService mediaService, ICommentService commentService)
        {
            _ps = postservice;
            _lg = logger;
            _ms = mediaService;
            _cs = commentService; 
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromRoute]NewPost post)
        {
            var media = post.MediaId.Select(id => _ms.GetAsync(id).Result);
            var result = await _ps.CreateAsync(post.ToEntity(media));
            
            if(result.IsSuccess)
            {
                _lg.LogInformation("Post created in db.");
                return CreatedAtAction(nameof(PostAsync), new{id = post.ToEntity(media).Id }, post.ToEntity(media));
            }

            return BadRequest(result.exception.Message);
        }

        
    }
}