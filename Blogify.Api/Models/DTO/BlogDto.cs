using System.ComponentModel.DataAnnotations;

namespace Blogify.Api.Models.DTO
{
    public class BlogDto
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PostDto> PostList { get; set; } = new();
    }
}
