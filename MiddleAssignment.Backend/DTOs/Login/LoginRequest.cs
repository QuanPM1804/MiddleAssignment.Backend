using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.DTOs.Login
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
