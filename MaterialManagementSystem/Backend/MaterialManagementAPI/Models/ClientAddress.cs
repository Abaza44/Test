using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a client address in the material management system
    /// Maps to the ClientAddresses table in the database
    /// </summary>
    [Table("ClientAddresses")]
    public class ClientAddress
    {
        /// <summary>
        /// Primary key for the client address
        /// </summary>
        [Key]
        public int AddressID { get; set; }

        /// <summary>
        /// Foreign key reference to the client
        /// </summary>
        [Required]
        public int ClientID { get; set; }

        /// <summary>
        /// The actual address text (required, max 200 characters)
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City name (optional, max 50 characters)
        /// </summary>
        [StringLength(50)]
        public string? City { get; set; }

        /// <summary>
        /// Indicates whether this is the default address for the client
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Navigation property to the parent client
        /// </summary>
        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; } = null!;

        /// <summary>
        /// Navigation property for sales invoices that use this address
        /// One address can be used in multiple sales invoices
        /// </summary>
        public virtual ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}

