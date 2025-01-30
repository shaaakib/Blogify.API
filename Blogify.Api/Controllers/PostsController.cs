using Blogify.Api.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Blogify.Api.Models.Domain;
using Blogify.Api.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Blogify.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Blogify.Api.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        
        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            var result = await _postRepository.CreatePostAsync(createPostDto);
            if (!result)
            {
                return BadRequest("Invalid BlogId or other error.");
            }

            return CreatedAtAction(nameof(GetPost), new { id = createPostDto.BlogId }, createPostDto);
        }

        [HttpPut("UpdatePost/{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto updatePostDto)
        {
            var result = await _postRepository.UpdatePostAsync(id, updatePostDto);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Post updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _postRepository.DeletePostAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok($"Post with ID {id} deleted successfully.");
        }
    }

}
