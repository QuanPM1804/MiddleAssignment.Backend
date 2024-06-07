using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.DTOs.Register
{
    public class RegistrationRequest
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
