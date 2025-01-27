using Blogify.Api.Data;
using Blogify.Api.Models.Domain;
using Blogify.Api.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blogify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogDbContext _db;
        public BlogsController(BlogDbContext db)
        {
            _db = db;
        }

        [HttpGet("/GetBlogs")]
        public async Task<IActionResult> GetBlogs()
        {

            // data query
            //var blogs = await _db.Blogs.Select(b => new
            //{
            //    b.BlogId,
            //    b.Name,
            //    b.Description,
            //    Post = b.Posts.Select(p => new
            //    {
            //        p.PostId,
            //        p.Title,
            //        p.Content
            //    })
            //}).ToListAsync();

            //Data Mapping after Fetching Complete Entity
            var blogs = await _db.Blogs
                       .Include(b => b.Posts) // Eager loading for related data
                       .ToListAsync();

            var blogDtos = blogs.Select(b => new BlogDto
            {
                BlogId = b.BlogId,
                Name = b.Name,
                Description = b.Description,
                PostList = b.Posts.Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content
                }).ToList()
            }).ToList();

            return Ok(blogs);
        }

        [HttpGet("GetBlog/{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _db.Blogs
                       .Include(b => b.Posts)
                       .FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog == null) return NotFound($"Record with ID {id} was not found");

            var blogDtos = new BlogDto
            {
                BlogId = blog.BlogId,
                Name = blog.Name,
                Description = blog.Description,
                PostList = blog.Posts.Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content
                }).ToList()

            };

            return Ok(blogDtos);
        }

        [HttpPost("CreateBlog")]
        public async Task<IActionResult> CreateBlog([FromBody]BlogDto blogDto)
        {
            var blog = new Blog
            {
                BlogId = blogDto.BlogId,
                Name = blogDto.Name,
                Description = blogDto.Description,
                Posts = blogDto.PostList.Select(p => new Post
                {
                    BlogId = p.PostId,
                    Title = p.Title,
                    Content = p.Content
                }).ToList()
            };

           await _db.Blogs.AddAsync(blog);
           await _db.SaveChangesAsync();
           return Ok(blog);
        }

        [HttpPut("UpdateBlog/{id}")]
        public async Task<IActionResult> UpdateBlog([FromBody]BlogDto blogDto, int id)
        {
            var blog = await _db.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog == null) return NotFound($"Record with ID {id} was not found");

            blog.Name = blogDto.Name;
            blog.Description = blogDto.Description;

            blog.Posts.Clear();

            foreach (var item in blogDto.PostList)
            {
                blog.Posts.Add(new Post
                {
                    Title = item.Title,
                    Content = item.Content
                });
            }

           await _db.SaveChangesAsync();


            return Ok(new {Message = "Blog updated successfully!" });
        }

        [HttpDelete("DeleteBlog/{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _db.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog == null) return NotFound($"Record with ID {id} was not found");

            _db.Blogs.Remove(blog);
            await _db.SaveChangesAsync();

            return Ok("Delete successfull");
        }

    }
}
