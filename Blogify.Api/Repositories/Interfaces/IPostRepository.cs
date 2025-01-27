using Blogify.Api.Models.Domain;
using Blogify.Api.Models.DTO;

namespace Blogify.Api.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<List<PostDtoReq>> GetPostsAsync();
        Task<PostDtoReq> GetPostByIdAsync(int id);
        Task<bool> CreatePostAsync(CreatePostDto createPostDto);
        Task<bool> UpdatePostAsync(int id, UpdatePostDto updatePostDto);
        Task<bool> DeletePostAsync(int id);
    }



}
