using AquaFlow.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaFlow.Models
{
    // Model representing an order in the AquaFlow application
    public class Order
    {
        public int OrderID { get; set; }  // Unique identifier for each order

        [Required]
        public virtual AquaFlowUser User { get; set; }  // User associated with the order

        [Required]
        public string Address { get; set; }  // Delivery address for the order

        [Required]
        public DateTime OrderDate { get; set; }  // Date and time when the order was placed

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }  // Total amount for the order

        [Required]
        public string Status { get; set; }  // Status of the order (e.g., processing, delivered)

        // Navigation property for OrderItems - represents the items associated with the order
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
