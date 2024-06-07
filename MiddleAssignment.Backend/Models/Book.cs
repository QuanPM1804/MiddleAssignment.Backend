using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public List<BookBorrowingRequestDetail> BookBorrowingRequestDetails { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
