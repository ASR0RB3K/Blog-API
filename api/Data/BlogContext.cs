using api.Entity;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<Media> Medias { get; set; }

        public BlogContext(DbContextOptions options)
            : base(options) { }
    }
}