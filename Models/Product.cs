using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaFlow.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Price { get; set; }

        public required int StockQuantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ImagePath { get; set; }
    }
}
