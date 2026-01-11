using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManager.Api.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}
