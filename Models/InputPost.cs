namespace BlogApi.Models
{
    public class InputPost
    {
        public IFormFile File { get; set; }
        public string Head { get; set; }
        public string Body { get; set; }
        public int AutorId { get; set; }
        public string? Path { get; set; }
    }
}
