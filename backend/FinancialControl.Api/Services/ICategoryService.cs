using FinancialControl.Api.DTOs;

namespace FinancialControl.Api.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync(Guid userId, Guid accountId);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid userId, Guid categoryId);
    Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryRequest request);
    Task<CategoryDto> UpdateCategoryAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request);
    Task DeleteCategoryAsync(Guid userId, Guid categoryId);
}
