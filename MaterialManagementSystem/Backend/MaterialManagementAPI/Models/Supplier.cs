using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a supplier in the material management system
    /// Maps to the Suppliers table in the database
    /// </summary>
    [Table("Suppliers")]
    public class Supplier
    {
        /// <summary>
        /// Primary key for the supplier
        /// </summary>
        [Key]
        public int SupplierID { get; set; }

        /// <summary>
        /// Supplier's name (required, max 100 characters)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Supplier's phone number (optional, max 20 characters)
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Supplier's address (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }

        /// <summary>
        /// Payment terms in days (default 30 days)
        /// Indicates how many days the supplier allows for payment
        /// </summary>
        public int PaymentTerms { get; set; } = 30;

        /// <summary>
        /// Indicates whether the supplier is active in the system
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property for purchase invoices
        /// One supplier can have multiple purchase invoices
        /// </summary>
        public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();

        /// <summary>
        /// Navigation property for payments made to this supplier
        /// One supplier can have multiple payment records
        /// </summary>
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}

