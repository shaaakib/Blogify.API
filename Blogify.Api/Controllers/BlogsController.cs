using Blogify.Api.Models.Domain;
using Blogify.Api.Models.DTO;
using Blogify.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IBlogRepository _blogRepository;

    public BlogController(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    [HttpGet("/GetBlogs")]
    public async Task<IActionResult> GetBlogs()
    {
        var blogs = await _blogRepository.GetAllBlogsAsync();
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

        return Ok(blogDtos);
    }

    [HttpGet("GetBlog/{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        var blog = await _blogRepository.GetBlogByIdAsync(id);
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
    public async Task<IActionResult> CreateBlog([FromBody] BlogDto blogDto)
    {
        var blog = new Blog
        {
            Name = blogDto.Name,
            Description = blogDto.Description,
            Posts = blogDto.PostList.Select(p => new Post
            {
                Title = p.Title,
                Content = p.Content
            }).ToList()
        };

        await _blogRepository.CreateBlogAsync(blog);
        return Ok(blog);
    }

    [HttpPut("UpdateBlog/{id}")]
    public async Task<IActionResult> UpdateBlog([FromBody] BlogDto blogDto, int id)
    {
        var blog = await _blogRepository.GetBlogByIdAsync(id);
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

        await _blogRepository.UpdateBlogAsync(blog);
        return Ok(new { Message = "Blog updated successfully!" });
    }

    [HttpDelete("DeleteBlog/{id}")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        var blog = await _blogRepository.GetBlogByIdAsync(id);
        if (blog == null) return NotFound($"Record with ID {id} was not found");

        await _blogRepository.DeleteBlogAsync(id);
        return Ok("Delete successful");
    }
}
