using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a material in the material management system
    /// Maps to the Materials table in the database
    /// </summary>
    [Table("Materials")]
    public class Material
    {
        /// <summary>
        /// Primary key for the material
        /// </summary>
        [Key]
        public int MaterialID { get; set; }

        /// <summary>
        /// Unique material code (required, max 20 characters)
        /// Used for quick identification and barcode scanning
        /// </summary>
        [Required]
        [StringLength(20)]
        public string MaterialCode { get; set; } = string.Empty;

        /// <summary>
        /// Material name (required, max 100 characters)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to material category (optional)
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Unit of measurement (required, max 20 characters)
        /// Examples: متر، كيلو، قطعة، شيكارة، طن
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Current stock quantity (calculated from stock batches)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentStock { get; set; } = 0;

        /// <summary>
        /// Minimum stock level for alerts
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal MinimumStock { get; set; } = 0;

        /// <summary>
        /// Current selling price per unit
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal SellingPrice { get; set; } = 0;

        /// <summary>
        /// Storage location in the warehouse (optional, max 100 characters)
        /// </summary>
        [StringLength(100)]
        public string? Location { get; set; }

        /// <summary>
        /// Indicates whether the material is active in the system
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property to the material category
        /// </summary>
        [ForeignKey("CategoryID")]
        public virtual MaterialCategory? Category { get; set; }

        /// <summary>
        /// Navigation property for stock batches (FIFO implementation)
        /// One material can have multiple stock batches
        /// </summary>
        public virtual ICollection<StockBatch> StockBatches { get; set; } = new List<StockBatch>();

        /// <summary>
        /// Navigation property for stock movements
        /// One material can have multiple stock movement records
        /// </summary>
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        /// <summary>
        /// Navigation property for sales invoice items
        /// One material can appear in multiple sales invoice items
        /// </summary>
        public virtual ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();

        /// <summary>
        /// Navigation property for purchase invoice items
        /// One material can appear in multiple purchase invoice items
        /// </summary>
        public virtual ICollection<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();
    }
}

