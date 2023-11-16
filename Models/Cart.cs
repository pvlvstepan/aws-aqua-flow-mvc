using AquaFlow.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace AquaFlow.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public required virtual AquaFlowUser User { get; set; }

        public required DateTime CreatedAt { get; set; }

        // Add this navigation property
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
