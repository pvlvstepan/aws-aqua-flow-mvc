using System.ComponentModel.DataAnnotations;

namespace AquaFlow.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public required virtual Cart Cart { get; set; }

        public required virtual Product Product { get; set; }

        public required int Quantity { get; set; }
    }
}
