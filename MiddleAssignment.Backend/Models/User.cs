using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public List<BookBorrowingRequest> BookBorrowingRequests { get; set; }
        public List<BookBorrowingRequest> BookBorrowingRequestsApproved { get; set; }
        public List<Review> Reviews { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string PasswordHash { get; set; }

    }
    public enum UserRole
    {
        SuperUser,
        User
    }
}
