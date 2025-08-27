using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a payment made to a supplier in the material management system
    /// Maps to the Payments table in the database
    /// </summary>
    [Table("Payments")]
    public class Payment
    {
        /// <summary>
        /// Primary key for the payment
        /// </summary>
        [Key]
        public int PaymentID { get; set; }

        /// <summary>
        /// Unique payment number (required, max 20 characters)
        /// Used for payment identification and reference
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PaymentNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to the supplier receiving the payment
        /// </summary>
        [Required]
        public int SupplierID { get; set; }

        /// <summary>
        /// Foreign key reference to a specific purchase invoice (optional)
        /// If null, this is a general payment to the supplier
        /// </summary>
        public int? PurchaseID { get; set; }

        /// <summary>
        /// Date when the payment was made
        /// </summary>
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Amount paid to the supplier
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Foreign key reference to the payment method used (optional)
        /// </summary>
        public int? PaymentMethodID { get; set; }

        /// <summary>
        /// Description of the payment (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property to the supplier
        /// </summary>
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { get; set; } = null!;

        /// <summary>
        /// Navigation property to the related purchase invoice (if any)
        /// </summary>
        [ForeignKey("PurchaseID")]
        public virtual PurchaseInvoice? Purchase { get; set; }

        /// <summary>
        /// Navigation property to the payment method
        /// </summary>
        [ForeignKey("PaymentMethodID")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}

