using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
