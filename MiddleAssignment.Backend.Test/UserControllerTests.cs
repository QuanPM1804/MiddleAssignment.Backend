using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MiddleAssignment.Server.Controllers;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO { Id = Guid.NewGuid(), Username = "user1", Email = "user1@example.com" },
                new UserDTO { Id = Guid.NewGuid(), Username = "user2", Email = "user2@example.com" }
            };
            _userServiceMock.Setup(service => service.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserDTO { Id = userId, Username = "user1", Email = "user1@example.com" };
            _userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDTO = new UserDTO { Id = userId, Username = "updateduser", Email = "updated@example.com" };
            _userServiceMock.Setup(service => service.UpdateUser(userId, userDTO)).ReturnsAsync(userDTO);

            // Act
            var result = await _controller.UpdateUser(userId, userDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(userDTO, okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDTO = new UserDTO { Id = userId, Username = "updateduser", Email = "updated@example.com" };
            _userServiceMock.Setup(service => service.UpdateUser(userId, userDTO)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.UpdateUser(userId, userDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContentResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(service => service.DeleteUser(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}