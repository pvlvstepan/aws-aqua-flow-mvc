using AquaFlow.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaFlow.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public required virtual AquaFlowUser User { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation property for OrderItems
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
