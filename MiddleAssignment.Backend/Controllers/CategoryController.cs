using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser, User")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperUser, User")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            var createdCategory = await _categoryService.CreateCategory(categoryDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> UpdateCategory(Guid id, CategoryDTO categoryDTO)
        {
            var updatedCategory = await _categoryService.UpdateCategory(id, categoryDTO);
            if (updatedCategory == null)
                return NotFound();

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
