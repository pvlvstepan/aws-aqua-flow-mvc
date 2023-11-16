using System.ComponentModel.DataAnnotations;

namespace AquaFlow.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
