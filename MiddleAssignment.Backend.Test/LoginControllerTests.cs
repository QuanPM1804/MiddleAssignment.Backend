using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MiddleAssignment.Server.Controllers;
using Xunit;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.DTOs.Register;
using MiddleAssignment.Backend.DTOs.Login;

namespace MiddleAssignment.Tests
{
    public class LoginControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new LoginController(_mockUserService.Object);
        }

        [Fact]
        public async Task Register_WithValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                Username = "testuser",
                Password = "testpassword",
                Email = "test@example.com"
            };
            var response = new RegistrationResponse { UserId = Guid.NewGuid() };
            _mockUserService.Setup(s => s.RegisterAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var registrationResponse = Assert.IsType<RegistrationResponse>(okResult.Value);
            Assert.Equal(response.UserId, registrationResponse.UserId);
        }

        [Fact]
        public async Task Register_WithInvalidRequest_ShouldReturnBadRequestResult()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                Username = "testuser",
                Password = "testpassword",
                Email = "test@example.com"
            };
            var errorMessage = "Invalid registration request.";
            _mockUserService.Setup(s => s.RegisterAsync(request)).ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task Login_WithValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "testpassword"
            };
            var response = new LoginResponse
            {
                AccessToken = "access_token",
                RefreshToken = "refresh_token",
                UserId = Guid.NewGuid()
            };
            _mockUserService.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var loginResponse = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal(response.AccessToken, loginResponse.AccessToken);
            Assert.Equal(response.RefreshToken, loginResponse.RefreshToken);
            Assert.Equal(response.UserId, loginResponse.UserId);
        }

        [Fact]
        public async Task Login_WithInvalidRequest_ShouldReturnUnauthorizedResult()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "testpassword"
            };
            var errorMessage = "Invalid login credentials.";
            _mockUserService.Setup(s => s.LoginAsync(request)).ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal(errorMessage, unauthorizedResult.Value);
        }
    }
}