namespace Blogify.Api.Models.DTO
{
    public class PostDtoReq
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public BlogDtoReq Blog { get; set; }
    }
}
