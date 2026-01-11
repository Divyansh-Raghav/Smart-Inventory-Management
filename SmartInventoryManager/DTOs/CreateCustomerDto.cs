using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManager.Api.DTOs
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}
