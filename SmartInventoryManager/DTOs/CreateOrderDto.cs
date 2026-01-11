using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManager.Api.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
