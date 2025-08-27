using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a stock movement record in the material management system
    /// Tracks all inventory movements (IN/OUT) for audit and reporting purposes
    /// Maps to the StockMovements table in the database
    /// </summary>
    [Table("StockMovements")]
    public class StockMovement
    {
        /// <summary>
        /// Primary key for the stock movement
        /// </summary>
        [Key]
        public int MovementID { get; set; }

        /// <summary>
        /// Foreign key reference to the material
        /// </summary>
        [Required]
        public int MaterialID { get; set; }

        /// <summary>
        /// Type of movement (required, max 10 characters)
        /// Values: "IN" (دخول) for incoming stock, "OUT" (خروج) for outgoing stock
        /// </summary>
        [Required]
        [StringLength(10)]
        public string MovementType { get; set; } = string.Empty;

        /// <summary>
        /// Quantity moved (positive for both IN and OUT)
        /// The MovementType indicates direction
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price at the time of movement (optional)
        /// For purchases: cost price, for sales: selling price
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Type of reference document (optional, max 20 characters)
        /// Values: "PURCHASE" (شراء), "SALE" (بيع), "ADJUSTMENT" (تسوية)
        /// </summary>
        [StringLength(20)]
        public string? ReferenceType { get; set; }

        /// <summary>
        /// ID of the reference document (optional)
        /// Links to PurchaseInvoice, SalesInvoice, or other documents
        /// </summary>
        public int? ReferenceID { get; set; }

        /// <summary>
        /// Date and time when the movement occurred
        /// </summary>
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Additional notes about the movement (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Notes { get; set; }

        /// <summary>
        /// Navigation property to the material
        /// </summary>
        [ForeignKey("MaterialID")]
        public virtual Material Material { get; set; } = null!;
    }
}

