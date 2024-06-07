using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MiddleAssignment.Server;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.Services.Implementations;
using MiddleAssignment.Backend.Server;
using MiddleAssignment.Backend.DTOs.Register;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.DTOs.Login;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly JwtSettings _jwtSettings;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            var jwtSettings = new JwtSettings(); // Provide necessary values for JwtSettings
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object, _jwtTokenServiceMock.Object, jwtSettings);
        }

        [Fact]
        public async Task RegisterAsync_WithValidRequest_ReturnsRegistrationResponse()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password"
            };
            var user = new User { Id = Guid.NewGuid(), Username = request.Username, Email = request.Email };
            var response = new RegistrationResponse { UserId = user.Id, Username = user.Username };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<RegistrationResponse>(user)).Returns(response);

            // Act
            var result = await _userService.RegisterAsync(request);

            // Assert
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task RegisterAsync_WithDuplicateUsername_ThrowsException()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                Username = "existinguser",
                Email = "newuser@example.com",
                Password = "password"
            };
            var existingUser = new User { Id = Guid.NewGuid(), Username = request.Username, Email = "other@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync(existingUser);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegisterAsync(request));
        }

        [Fact]
        public async Task RegisterAsync_WithDuplicateEmail_ThrowsException()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                Username = "newuser",
                Email = "existinguser@example.com",
                Password = "password"
            };
            var existingUser = new User { Id = Guid.NewGuid(), Username = "other", Email = request.Email };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email)).ReturnsAsync(existingUser);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegisterAsync(request));
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var request = new LoginRequest { Username = "user", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), Username = request.Username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password) };
            var accessToken = "access_token";
            var refreshToken = "refresh_token";
            var response = new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken, UserId = user.Id };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync(user);
            _jwtTokenServiceMock.Setup(service => service.GenerateAccessToken(user)).Returns(accessToken);
            _jwtTokenServiceMock.Setup(service => service.GenerateRefreshToken()).Returns(refreshToken);
            _userRepositoryMock.Setup(repo => repo.UpdateUser(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(request);

            // Assert
            Assert.Equal(response.AccessToken, result.AccessToken);
            Assert.Equal(response.RefreshToken, result.RefreshToken);
            Assert.Equal(response.UserId, result.UserId);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidUsername_ThrowsException()
        {
            // Arrange
            var request = new LoginRequest { Username = "invaliduser", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync((User)null);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ThrowsException()
        {
            // Arrange
            var request = new LoginRequest { Username = "user", Password = "invalidpassword" };
            var user = new User { Id = Guid.NewGuid(), Username = request.Username, PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword") };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync(user);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.LoginAsync(request));
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnUserDTOs()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Username = "user1", Email = "user1@example.com", Role = UserRole.User },
                new User { Id = Guid.NewGuid(), Username = "user2", Email = "user2@example.com", Role = UserRole.User }
            };

            var userDTOs = new List<UserDTO>
            {
                new UserDTO { Id = users[0].Id, Username = users[0].Username, Email = users[0].Email, Role = users[0].Role },
                new UserDTO { Id = users[1].Id, Username = users[1].Username, Email = users[1].Email, Role = users[1].Role }
            };

            _userRepositoryMock.Setup(r => r.GetAllUsers()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(users)).Returns(userDTOs);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.Equal(userDTOs, result);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "testuser", Email = "testuser@example.com", Role = UserRole.User };
            var userDTO = new UserDTO { Id = user.Id, Username = user.Username, Email = user.Email, Role = user.Role };

            _userRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.Equal(userDTO, result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnUpdatedUserDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updatedUserDTO = new UserDTO { Id = userId, Username = "updateduser", Email = "updateduser@example.com", Role = UserRole.User };
            var existingUser = new User { Id = userId, Username = "testuser", Email = "testuser@example.com", Role = UserRole.User };
            var updatedUser = new User { Id = userId, Username = updatedUserDTO.Username, Email = updatedUserDTO.Email, Role = updatedUserDTO.Role };

            _userRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateUser(It.IsAny<User>())).ReturnsAsync(updatedUser);
            _mapperMock.Setup(m => m.Map(updatedUserDTO, existingUser)).Verifiable();
            _mapperMock.Setup(m => m.Map<UserDTO>(updatedUser)).Returns(updatedUserDTO);

            // Act
            var result = await _userService.UpdateUser(userId, updatedUserDTO);

            // Assert
            Assert.Equal(updatedUserDTO, result);
            _mapperMock.Verify();
        }

        [Fact]
        public async Task DeleteUser_ShouldDeleteUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await _userService.DeleteUser(userId);

            // Assert
            _userRepositoryMock.Verify(r => r.DeleteUser(userId), Times.Once);
        }
    }
}