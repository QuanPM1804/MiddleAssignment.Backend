using MiddleAssignment.Backend.Models;
using System.Security.Claims;

namespace MiddleAssignment.Backend.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
