using Blogify.Api.Data;
using Blogify.Api.Models.Domain;
using Blogify.Api.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogDbContext _db;
        public PostsController(BlogDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _db.Posts
        .Include(p => p.Blog)  // Include Blog details
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

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _db.Posts
        .Include(p => p.Blog)  // Include Blog details
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

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // POST: api/Posts
        [HttpPost("CreatePost")]
        public async Task<ActionResult<Post>> CreatePost(CreatePostDto createPostDto)
        {
            // Validate if the blog exists
            var blogExists = await _db.Blogs.AnyAsync(b => b.BlogId == createPostDto.BlogId);
            if (!blogExists)
            {
                return BadRequest("Invalid BlogId. Blog does not exist.");
            }

            // Map DTO to the Post entity
            var post = new Post
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                BlogId = createPostDto.BlogId
            };

            // Add to the database
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            // Return created post with location
            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }

        // PUT: api/Posts/UpdatePost/{id}
        [HttpPut("UpdatePost/{id}")]
        public async Task<IActionResult> UpdatePost(int id, UpdatePostDto updatePostDto)
        {
            // Validate if the post exists
            var existingPost = await _db.Posts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound("Post not found.");
            }

            // Validate if the blog exists
            var blogExists = await _db.Blogs.AnyAsync(b => b.BlogId == updatePostDto.BlogId);
            if (!blogExists)
            {
                return BadRequest("Invalid BlogId. Blog does not exist.");
            }

            // Update the post entity
            existingPost.Title = updatePostDto.Title;
            existingPost.Content = updatePostDto.Content;
            existingPost.BlogId = updatePostDto.BlogId;

            // Save changes to the database
            await _db.SaveChangesAsync();

            return Ok(existingPost); // Return 204 No Content for successful update
        }


        // DELETE: api/Posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            // Find the post by ID
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            // Remove the post
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();

            return Ok($"Post with ID {id} deleted successfully.");
        }
    }
}
