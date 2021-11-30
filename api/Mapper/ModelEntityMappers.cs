using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Mappers
{
    public static class ModelEntityMappers
    {
        public static Entity.Media GetMediaEntity(this IFormFile media)
        {
            using var stream = new MemoryStream();

            media.CopyTo(stream);
            return new Entity.Media(
                contentType: media.ContentType,
                data: stream.ToArray()
            );
        }
    }
}