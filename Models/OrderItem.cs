using System.ComponentModel.DataAnnotations;
using AquaFlow.Models;

namespace AquaFlow.Models
{
    // Model representing an item within an order in the AquaFlow application
    public class OrderItem
    {
        public int OrderItemID { get; set; }  // Unique identifier for each order item

        [Required]
        public int Quantity { get; set; }  // Quantity of the product in the order item

        // Navigation properties
        public virtual Order Order { get; set; }  // Reference to the order associated with the order item

        public virtual Product Product { get; set; }  // Reference to the product associated with the order item
    }
}
