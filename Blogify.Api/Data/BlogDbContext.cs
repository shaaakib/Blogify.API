using Blogify.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogify.Api.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasMany(s => s.Posts)
                .WithOne(p => p.Blog)
                .HasForeignKey(p => p.BlogId);
        }
    }
}
