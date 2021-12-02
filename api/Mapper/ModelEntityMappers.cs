using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using api.Entity;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Mappers
{
    public static class ModelEntityMappers
    {
        public static Post ToEntity(this NewPost post)
            => new Post()
            {
                Id = Guid.NewGuid(),
                HandlerImageId = post.HandlerImageId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Comments = null,
                Medias = null,
            };
        public static Media GetMediaEntity(this IFormFile media)
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