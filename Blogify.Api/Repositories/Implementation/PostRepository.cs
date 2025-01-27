using Blogify.Api.Data;
using Blogify.Api.Models.Domain;
using Blogify.Api.Models.DTO;
using Blogify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blogify.Api.Repositories.Implementation
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext _db;

        public PostRepository(BlogDbContext db)
        {
            _db = db;
        }

        public async Task<List<PostDtoReq>> GetPostsAsync()
        {
            return await _db.Posts
                .Include(p => p.Blog)
                .Select(p => new PostDtoReq
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    Blog = new BlogDtoReq
                    {
                        Name = p.Blog.Name,
                        Description = p.Blog.Description
                    }
                })
                .ToListAsync();
        }

        public async Task<PostDtoReq> GetPostByIdAsync(int id)
        {
            var post = await _db.Posts
                 .Include(p => p.Blog)
                 .Where(p => p.PostId == id)
                 .Select(p => new PostDtoReq
                 {
                     PostId = p.PostId,
                     Title = p.Title,
                     Content = p.Content,
                     Blog = new BlogDtoReq
                     {
                         Name = p.Blog.Name,
                         Description = p.Blog.Description
                     }
                 })
                 .FirstOrDefaultAsync();

            return post;
        }

        public async Task<bool> CreatePostAsync(CreatePostDto createPostDto)
        {
            var blogExists = await _db.Blogs.AnyAsync(b => b.BlogId == createPostDto.BlogId);
            if (!blogExists)
                return false;

            var post = new Post
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                BlogId = createPostDto.BlogId
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdatePostDto updatePostDto)
        {
            var existingPost = await _db.Posts.FindAsync(id);
            if (existingPost == null)
                return false;

            var blogExists = await _db.Blogs.AnyAsync(b => b.BlogId == updatePostDto.BlogId);
            if (!blogExists)
                return false;

            existingPost.Title = updatePostDto.Title;
            existingPost.Content = updatePostDto.Content;
            existingPost.BlogId = updatePostDto.BlogId;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
                return false;

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
