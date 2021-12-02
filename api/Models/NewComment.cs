using System;

namespace api.Models
{
    public class NewComment
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public EStateNew State { get; set; }
        public Guid PostId { get; set; }
    }
}