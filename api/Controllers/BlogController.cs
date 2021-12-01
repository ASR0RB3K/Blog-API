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
        private readonly IPostService _ps;
        private readonly ILogger<BlogController> _lg;
        private readonly IMediaService _ms;

        public BlogController(IPostService postservice, ILogger<BlogController> logger, IMediaService mediaService)
        {
            _ps = postservice;
            _lg = logger;
            _ms = mediaService;
        }
    }
}