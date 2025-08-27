using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a stock batch in the material management system
    /// This is the core entity for FIFO (First In, First Out) inventory management
    /// Maps to the StockBatches table in the database
    /// </summary>
    [Table("StockBatches")]
    public class StockBatch
    {
        /// <summary>
        /// Primary key for the stock batch
        /// </summary>
        [Key]
        public int BatchID { get; set; }

        /// <summary>
        /// Foreign key reference to the material
        /// </summary>
        [Required]
        public int MaterialID { get; set; }

        /// <summary>
        /// Foreign key reference to the purchase invoice (optional)
        /// Links this batch to the purchase that created it
        /// </summary>
        public int? PurchaseID { get; set; }

        /// <summary>
        /// Batch number for identification (optional, max 20 characters)
        /// Can be used for lot tracking
        /// </summary>
        [StringLength(20)]
        public string? BatchNumber { get; set; }

        /// <summary>
        /// Date when this batch was purchased/received
        /// Used for FIFO ordering (oldest first)
        /// </summary>
        [Required]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Original quantity when the batch was first received
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal InitialQuantity { get; set; }

        /// <summary>
        /// Current remaining quantity in this batch
        /// Decreases as materials are sold (FIFO)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal RemainingQuantity { get; set; }

        /// <summary>
        /// Unit cost for this specific batch
        /// Used to calculate cost of goods sold in FIFO
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Expiry date for the batch (optional)
        /// Used for materials that have expiration dates
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Additional notes about this batch (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Notes { get; set; }

        /// <summary>
        /// Navigation property to the material
        /// </summary>
        [ForeignKey("MaterialID")]
        public virtual Material Material { get; set; } = null!;

        /// <summary>
        /// Navigation property to the purchase invoice that created this batch
        /// </summary>
        [ForeignKey("PurchaseID")]
        public virtual PurchaseInvoice? PurchaseInvoice { get; set; }

        /// <summary>
        /// Navigation property for sales cost details
        /// Tracks which sales used quantities from this batch
        /// </summary>
        public virtual ICollection<SalesCostDetail> SalesCostDetails { get; set; } = new List<SalesCostDetail>();
    }
}

