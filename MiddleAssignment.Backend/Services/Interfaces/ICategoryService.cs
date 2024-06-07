using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(Guid id);
        Task<CategoryDTO> CreateCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO> UpdateCategory(Guid id, CategoryDTO categoryDTO);
        Task DeleteCategory(Guid id);
    }
}
