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

        public static EState ToEntityEComment(this EStateNew? State)
        {
            return State switch
            {
                Models.EStateNew.Pending => Entity.EState.Pending,
                Models.EStateNew.Approved => Entity.EState.Approved,
                _ => Entity.EState.Rejected,
            };
        } 

        public static Comment ToCommentEntity(this NewComment comment)
            => new Entity.Comment()
            {
                Id = Guid.NewGuid(),
                Author = comment.Author,
                Content = comment.Content,
                State = ToEntityEComment(comment.State),
                PostId= comment.PostId
            };  
    }
}