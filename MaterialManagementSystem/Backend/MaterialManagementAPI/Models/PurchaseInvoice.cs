using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a purchase invoice in the material management system
    /// Maps to the PurchaseInvoices table in the database
    /// </summary>
    [Table("PurchaseInvoices")]
    public class PurchaseInvoice
    {
        /// <summary>
        /// Primary key for the purchase invoice
        /// </summary>
        [Key]
        public int PurchaseID { get; set; }

        /// <summary>
        /// Unique purchase invoice number (required, max 20 characters)
        /// Used for purchase identification and reference
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PurchaseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to the supplier
        /// </summary>
        [Required]
        public int SupplierID { get; set; }

        /// <summary>
        /// Date when the purchase was made
        /// </summary>
        public DateTime PurchaseDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Total amount of the purchase invoice
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        /// <summary>
        /// Amount already paid to the supplier
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        /// <summary>
        /// Additional notes for the purchase (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? Notes { get; set; }

        /// <summary>
        /// Path to the supplier's invoice image/PDF file (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? InvoiceImagePath { get; set; }

        /// <summary>
        /// Navigation property to the supplier
        /// </summary>
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { get; set; } = null!;

        /// <summary>
        /// Navigation property for purchase invoice items
        /// One purchase invoice can have multiple items
        /// </summary>
        public virtual ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();

        /// <summary>
        /// Navigation property for stock batches created from this purchase
        /// One purchase can create multiple stock batches
        /// </summary>
        public virtual ICollection<StockBatch> StockBatches { get; set; } = new List<StockBatch>();

        /// <summary>
        /// Navigation property for payments related to this purchase
        /// One purchase can have multiple payment records
        /// </summary>
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}

