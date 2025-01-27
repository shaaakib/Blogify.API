using Blogify.Api.Data;
using Blogify.Api.Models.Domain;
using Blogify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _db;

    public BlogRepository(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _db.Blogs.Include(b => b.Posts).ToListAsync();
    }

    public async Task<Blog> GetBlogByIdAsync(int id)
    {
         var blog = await _db.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.BlogId == id);
        return blog;
    }

    public async Task CreateBlogAsync(Blog blog)
    {
        await _db.Blogs.AddAsync(blog);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateBlogAsync(Blog blog)
    {
        _db.Blogs.Update(blog);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteBlogAsync(int id)
    {
        var blog = await _db.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.BlogId == id);
        if (blog != null)
        {
            _db.Blogs.Remove(blog);
            await _db.SaveChangesAsync();
        }
    }
}
