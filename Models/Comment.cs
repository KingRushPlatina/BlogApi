
namespace BlogApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string CreationDate { get; set; }

        public Autor Commentator { get; set; }
    }
}
