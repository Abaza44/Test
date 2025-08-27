using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents an invoice status in the material management system
    /// Maps to the InvoiceStatuses table in the database
    /// </summary>
    [Table("InvoiceStatuses")]
    public class InvoiceStatus
    {
        /// <summary>
        /// Primary key for the invoice status
        /// </summary>
        [Key]
        public int StatusID { get; set; }

        /// <summary>
        /// Status name (required, max 50 characters)
        /// Examples: مسودة، مؤكدة، مدفوعة جزئياً، مدفوعة كاملة، ملغية
        /// </summary>
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for sales invoices with this status
        /// One status can be used by multiple sales invoices
        /// </summary>
        public virtual ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}

