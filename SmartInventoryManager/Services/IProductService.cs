using SmartInventoryManager.Api.DTOs;

namespace SmartInventoryManager.Api.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task UpdateAsync(int id, CreateProductDto dto);
        Task DeleteAsync(int id);
    }
}
