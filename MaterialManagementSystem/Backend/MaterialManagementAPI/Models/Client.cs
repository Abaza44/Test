using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a client in the material management system
    /// Maps to the Clients table in the database
    /// </summary>
    [Table("Clients")]
    public class Client
    {
        /// <summary>
        /// Primary key for the client
        /// </summary>
        [Key]
        public int ClientID { get; set; }

        /// <summary>
        /// Client's full name (required, max 100 characters)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Client's phone number (required, unique, max 20 characters)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Date when the client record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the client is active in the system
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property for client addresses
        /// One client can have multiple addresses
        /// </summary>
        public virtual ICollection<ClientAddress> Addresses { get; set; } = new List<ClientAddress>();

        /// <summary>
        /// Navigation property for sales invoices
        /// One client can have multiple sales invoices
        /// </summary>
        public virtual ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();

        /// <summary>
        /// Navigation property for collections (payments received from client)
        /// One client can have multiple collection records
        /// </summary>
        public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}

