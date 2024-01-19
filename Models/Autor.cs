using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
    }
}
