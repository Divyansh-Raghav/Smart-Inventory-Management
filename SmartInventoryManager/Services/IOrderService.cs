using SmartInventoryManager.Api.DTOs;

namespace SmartInventoryManager.Api.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
    }
}
