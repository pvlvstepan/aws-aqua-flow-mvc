using AquaFlow.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

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
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation property for OrderItems
        public List<OrderItem> OrderItems { get; set; }
    }
}
