using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using MiddleAssignment.Backend.Services.Implementations;
using MiddleAssignment.Backend.Server;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Server.Tests
{
    public class JwtTokenServiceTests
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenServiceTests()
        {
            _jwtSettings = new JwtSettings
            {
                SecretKey = "ThisIsAVerySecureSecretKey",
                Issuer = "MiddleAssignmentServer",
                Audience = "MiddleAssignmentClient",
                AccessTokenExpirationMinutes = 60
            };

            _jwtTokenService = new JwtTokenService(_jwtSettings);
        }

        [Fact]
        public void GenerateAccessToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                Role = UserRole.SuperUser
            };

            // Act
            var accessToken = _jwtTokenService.GenerateAccessToken(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
            };

            var claimsPrincipal = handler.ValidateToken(accessToken, validationParameters, out var _);
            Assert.NotNull(claimsPrincipal);
            Assert.Equal(user.Id.ToString(), claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            Assert.Equal(user.Username, claimsPrincipal.FindFirstValue(ClaimTypes.Name));
            Assert.Equal(user.Role.ToString(), claimsPrincipal.FindFirstValue(ClaimTypes.Role));
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnNonEmptyString()
        {
            // Act
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(refreshToken);
            Assert.NotEmpty(refreshToken);
        }

        [Fact]
        public void GetPrincipalFromExpiredToken_ShouldThrowSecurityTokenException()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                Role = UserRole.SuperUser
            };
            var expiredToken = _jwtTokenService.GenerateAccessToken(user);

            // Act & Assert
            Assert.Throws<SecurityTokenException>(() => _jwtTokenService.GetPrincipalFromExpiredToken(expiredToken));
        }

        [Fact]
        public void GenerateAccessTokenFromUserDto_ShouldReturnValidJwtToken()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                Role = UserRole.SuperUser
            };

            // Act
            var accessToken = _jwtTokenService.GenerateAccessToken(new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                Role = userDto.Role
            });

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
            };

            var claimsPrincipal = handler.ValidateToken(accessToken, validationParameters, out var _);
            Assert.NotNull(claimsPrincipal);
            Assert.Equal(userDto.Id.ToString(), claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            Assert.Equal(userDto.Username, claimsPrincipal.FindFirstValue(ClaimTypes.Name));
            Assert.Equal(userDto.Role.ToString(), claimsPrincipal.FindFirstValue(ClaimTypes.Role));
        }
    }
}