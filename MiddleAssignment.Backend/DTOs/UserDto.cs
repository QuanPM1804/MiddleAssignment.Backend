using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
