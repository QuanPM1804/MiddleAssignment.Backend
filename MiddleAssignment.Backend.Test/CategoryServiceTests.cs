using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Implementations;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsListOfCategoryDTOs()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category 1" },
                new Category { Id = Guid.NewGuid(), Name = "Category 2" }
            };
            var categoryDTOs = new List<CategoryDTO>
            {
                new CategoryDTO { Id = categories[0].Id, Name = categories[0].Name },
                new CategoryDTO { Id = categories[1].Id, Name = categories[1].Name }
            };
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(categories);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CategoryDTO>>(categories)).Returns(categoryDTOs);

            // Act
            var result = await _categoryService.GetAllCategories();

            // Assert
            Assert.Equal(categoryDTOs, result);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategoryDTO()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Category 1" };
            var categoryDTO = new CategoryDTO { Id = category.Id, Name = category.Name };
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(categoryId)).ReturnsAsync(category);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDTO>(category)).Returns(categoryDTO);

            // Act
            var result = await _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.Equal(categoryDTO, result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCreatedCategoryDTO()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "New Category" };
            var category = new Category { Name = categoryDTO.Name };
            var createdCategory = new Category { Id = Guid.NewGuid(), Name = categoryDTO.Name };
            var createdCategoryDTO = new CategoryDTO { Id = createdCategory.Id, Name = createdCategory.Name };
            _mapperMock.Setup(mapper => mapper.Map<Category>(categoryDTO)).Returns(category);
            _categoryRepositoryMock.Setup(repo => repo.CreateCategory(category)).ReturnsAsync(createdCategory);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDTO>(createdCategory)).Returns(createdCategoryDTO);

            // Act
            var result = await _categoryService.CreateCategory(categoryDTO);

            // Assert
            Assert.Equal(createdCategoryDTO, result);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsUpdatedCategoryDTO()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryDTO = new CategoryDTO { Id = categoryId, Name = "Updated Category" };
            var category = new Category { Id = categoryId, Name = "Category 1" };
            var updatedCategory = new Category { Id = categoryId, Name = categoryDTO.Name };
            var updatedCategoryDTO = new CategoryDTO { Id = updatedCategory.Id, Name = updatedCategory.Name };
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(categoryId)).ReturnsAsync(category);
            _categoryRepositoryMock.Setup(repo => repo.UpdateCategory(updatedCategory)).ReturnsAsync(updatedCategory);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDTO>(updatedCategory)).Returns(updatedCategoryDTO);

            // Act
            var result = await _categoryService.UpdateCategory(categoryId, categoryDTO);

            // Assert
            Assert.Equal(updatedCategoryDTO, result);
        }

        [Fact]
        public async Task UpdateCategory_WithNonExistingCategory_ReturnsNull()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryDTO = new CategoryDTO { Id = categoryId, Name = "Updated Category" };
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.UpdateCategory(categoryId, categoryDTO);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCategory_CallsDeleteCategoryOnRepository()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryRepositoryMock.Setup(repo => repo.DeleteCategory(categoryId)).Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteCategory(categoryId);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.DeleteCategory(categoryId), Times.Once);
        }
    }
}