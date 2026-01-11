using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventoryManager.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
       
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
       
    }
}

