using BlogApi.Models;

namespace BlogApi.Service.Interface
{
    public interface IAuthorService
    {
        Task<Autor?> GetPostById(int id);
    }
}
