using BlogApi.Models;

namespace BlogApi.Service.Interface
{
    public interface IAuthorService
    {
        public Autor GetPostById(int id);
    }
}
