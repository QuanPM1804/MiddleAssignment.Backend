using AutoMapper;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Backend.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> CreateCategory(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            var createdCategory = await _categoryRepository.CreateCategory(category);
            return _mapper.Map<CategoryDTO>(createdCategory);
        }

        public async Task<CategoryDTO> UpdateCategory(Guid id, CategoryDTO categoryDTO)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
                return null;

            _mapper.Map(categoryDTO, category);
            var updatedCategory = await _categoryRepository.UpdateCategory(category);
            return _mapper.Map<CategoryDTO>(updatedCategory);
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
                return;

            if (category.Books.Any())
            {
                throw new BadHttpRequestException("Cannot delete a category that has associated books.");
            }

            await _categoryRepository.DeleteCategory(id);
        }
    }
}
