using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string PublishDate { get; set; }
        public Autor Autor { get; set; }
        [AllowNull]
        public List<Comment>? Comments { get; set; }
        public string? ImagePath { get; set; }
    }
}
