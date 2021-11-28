using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Entity
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid HandlerImageId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        public uint Viewed { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Media> Medias { get; set; }

        [Obsolete("Used only for Entities binding.", true)]
        public Post() { }

        public Post(Guid handlerImageId, string title, string description, string content, uint viewed, DateTimeOffset createdAt, DateTimeOffset modifiedAt)
        {
            Id = Guid.NewGuid();
            HandlerImageId = handlerImageId;
            Title = title;
            Description = description;
            Content = content;
            Viewed = viewed;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }
    }
}