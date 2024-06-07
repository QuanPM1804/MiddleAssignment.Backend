using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MiddleAssignment.Server.Controllers;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult()
        {
            // Arrange
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = Guid.NewGuid(), Name = "Category 1" },
                new CategoryDTO { Id = Guid.NewGuid(), Name = "Category 2" }
            };
            _categoryServiceMock.Setup(service => service.GetAllCategories()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(categories, okResult.Value);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsOkResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new CategoryDTO { Id = categoryId, Name = "Category 1" };
            _categoryServiceMock.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(category, okResult.Value);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync((CategoryDTO)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "New Category" };
            var createdCategory = new CategoryDTO { Id = Guid.NewGuid(), Name = "New Category" };
            _categoryServiceMock.Setup(service => service.CreateCategory(categoryDTO)).ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.CreateCategory(categoryDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.Equal(createdCategory, createdAtActionResult.Value);
            Assert.Equal("GetCategoryById", createdAtActionResult.ActionName);
            Assert.Equal(createdCategory.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryDTO = new CategoryDTO { Id = categoryId, Name = "Updated Category" };
            _categoryServiceMock.Setup(service => service.UpdateCategory(categoryId, categoryDTO)).ReturnsAsync(categoryDTO);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(categoryDTO, okResult.Value);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryDTO = new CategoryDTO { Id = categoryId, Name = "Updated Category" };
            _categoryServiceMock.Setup(service => service.UpdateCategory(categoryId, categoryDTO)).ReturnsAsync((CategoryDTO)null);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContentResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(service => service.DeleteCategory(categoryId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}