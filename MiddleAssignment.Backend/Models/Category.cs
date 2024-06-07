using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
