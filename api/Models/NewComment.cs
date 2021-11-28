using System;

namespace api.Models
{
    public class NewModel
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public EStateNew State { get; set; }
        public Guid Id { get; set; }
    }
}