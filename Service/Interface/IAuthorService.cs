using BlogApi.Models;

namespace BlogApi.Service.Interface
{
    public interface IAuthorService
    {
        public void Conn();
        public string GetPostById(int id);
    }
}
