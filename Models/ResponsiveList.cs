namespace BlogApi.Models
{
    public class ResponsiveList<T>
    {
        public int PageSize { get; set; } = 4;
        public int PageNumber { get; set; }
        public List<T> List { get; set; }
    }
}
