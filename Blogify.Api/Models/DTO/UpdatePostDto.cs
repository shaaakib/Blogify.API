namespace Blogify.Api.Models.DTO
{
    public class UpdatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
    }

}
