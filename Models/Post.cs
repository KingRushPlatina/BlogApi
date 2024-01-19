using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlogApi.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateOnly PublishDate { get; set; }
        Autor Autor { get; set; }
        [AllowNull]
        List<Comment>? Comments { get; set; }
    }
}
