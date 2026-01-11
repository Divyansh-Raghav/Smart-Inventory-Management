using SmartInventoryManager.Api.DTOs;

namespace SmartInventoryManager.Api.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDto>> GetAllAsync();
        Task<CustomerResponseDto?> GetByIdAsync(int id);
        Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto);
        Task UpdateAsync(int id, CreateCustomerDto dto);
        Task DeleteAsync(int id);
    }
}
