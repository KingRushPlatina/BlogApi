using BlogApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApi.Service.Interface
{
    public interface IBlogService
    {
        Task AddPost(Post value);
        Task<Post> GetPostById(int id);
        Task<List<Post>> GetPosts(string title, int pageNumber, int pageSize);
    }
}
