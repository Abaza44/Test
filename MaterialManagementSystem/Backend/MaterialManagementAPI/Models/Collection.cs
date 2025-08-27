using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a collection (payment received from client) in the material management system
    /// Maps to the Collections table in the database
    /// </summary>
    [Table("Collections")]
    public class Collection
    {
        /// <summary>
        /// Primary key for the collection
        /// </summary>
        [Key]
        public int CollectionID { get; set; }

        /// <summary>
        /// Unique collection number (required, max 20 characters)
        /// Used for collection identification and reference
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CollectionNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to the client making the payment
        /// </summary>
        [Required]
        public int ClientID { get; set; }

        /// <summary>
        /// Foreign key reference to a specific invoice (optional)
        /// If null, this is a payment on account
        /// </summary>
        public int? InvoiceID { get; set; }

        /// <summary>
        /// Date when the collection was made
        /// </summary>
        public DateTime CollectionDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Amount collected from the client
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Foreign key reference to the payment method used (optional)
        /// </summary>
        public int? PaymentMethodID { get; set; }

        /// <summary>
        /// Additional notes about the collection (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Notes { get; set; }

        /// <summary>
        /// Navigation property to the client
        /// </summary>
        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; } = null!;

        /// <summary>
        /// Navigation property to the related invoice (if any)
        /// </summary>
        [ForeignKey("InvoiceID")]
        public virtual SalesInvoice? Invoice { get; set; }

        /// <summary>
        /// Navigation property to the payment method
        /// </summary>
        [ForeignKey("PaymentMethodID")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}

