using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a sales invoice in the material management system
    /// Maps to the SalesInvoices table in the database
    /// </summary>
    [Table("SalesInvoices")]
    public class SalesInvoice
    {
        /// <summary>
        /// Primary key for the sales invoice
        /// </summary>
        [Key]
        public int InvoiceID { get; set; }

        /// <summary>
        /// Unique invoice number (required, max 20 characters)
        /// Used for invoice identification and reference
        /// </summary>
        [Required]
        [StringLength(20)]
        public string InvoiceNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to the client
        /// </summary>
        [Required]
        public int ClientID { get; set; }

        /// <summary>
        /// Foreign key reference to the delivery address (optional)
        /// </summary>
        public int? AddressID { get; set; }

        /// <summary>
        /// Date when the invoice was created
        /// </summary>
        public DateTime InvoiceDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Due date for payment (optional)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Foreign key reference to invoice status (default: 1 for مسودة)
        /// </summary>
        public int StatusID { get; set; } = 1;

        /// <summary>
        /// Subtotal before discount
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; } = 0;

        /// <summary>
        /// Discount amount applied to the invoice
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        /// <summary>
        /// Total amount after discount
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        /// <summary>
        /// Amount already paid by the client
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        /// <summary>
        /// Additional notes for the invoice (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? Notes { get; set; }

        /// <summary>
        /// Path to the invoice image/PDF file (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? InvoiceImagePath { get; set; }

        /// <summary>
        /// Navigation property to the client
        /// </summary>
        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; } = null!;

        /// <summary>
        /// Navigation property to the delivery address
        /// </summary>
        [ForeignKey("AddressID")]
        public virtual ClientAddress? Address { get; set; }

        /// <summary>
        /// Navigation property to the invoice status
        /// </summary>
        [ForeignKey("StatusID")]
        public virtual InvoiceStatus Status { get; set; } = null!;

        /// <summary>
        /// Navigation property for invoice items
        /// One invoice can have multiple items
        /// </summary>
        public virtual ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();

        /// <summary>
        /// Navigation property for collections related to this invoice
        /// One invoice can have multiple collection records
        /// </summary>
        public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}

