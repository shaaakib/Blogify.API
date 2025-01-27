using Blogify.Api.Models.Domain;

namespace Blogify.Api.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetAllBlogsAsync();
        Task<Blog> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(int id);
    }

}
