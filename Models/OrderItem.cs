using System.ComponentModel.DataAnnotations;

namespace AquaFlow.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
