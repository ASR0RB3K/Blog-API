using System.IO;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using api.Mappers;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mds;

        public MediaController(IMediaService mediaservice)
        {
            _mds = mediaservice;
        }

        [HttpPost]
        public async Task<ActionResult> PostMedia([FromForm]NewMedia media)
        {
            if(ModelState.IsValid)
            {
                var images = media.Data.Select(m => m.GetMediaEntity()).ToList();

                // images.ForEach(m => _mds.ExistAsync(images));

                await _mds.InsertAsync(images);

                return Ok(images.Select(m => new { 
                    Id = m.Id,
                    ContentType = m.ContentType
                    }).ToList());
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet]
        public async Task<IActionResult>  GetAllAsync()
        {
            var medias = await _mds.GetAllAsync();
            var result = medias.Select(u => new {
                Id = u.Id,
                ContentType = u.ContentType,
                });
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var file = await _mds.GetAsync(id);
            var stream = new MemoryStream(file.Data);
            return File(stream.ToArray(), file.ContentType);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _mds.DeleteAsync(id);
            if(result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
