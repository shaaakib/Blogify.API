using System.ComponentModel.DataAnnotations;

namespace Blogify.Api.Models.DTO
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
