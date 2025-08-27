using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents an item within a purchase invoice
    /// Maps to the PurchaseInvoiceItems table in the database
    /// </summary>
    [Table("PurchaseInvoiceItems")]
    public class PurchaseInvoiceItem
    {
        /// <summary>
        /// Primary key for the purchase invoice item
        /// </summary>
        [Key]
        public int ItemID { get; set; }

        /// <summary>
        /// Foreign key reference to the purchase invoice
        /// </summary>
        [Required]
        public int PurchaseID { get; set; }

        /// <summary>
        /// Foreign key reference to the material being purchased
        /// </summary>
        [Required]
        public int MaterialID { get; set; }

        /// <summary>
        /// Quantity of the material being purchased
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit purchase price for this item
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Line total (calculated as Quantity * UnitPrice)
        /// This is a computed column in the database
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal LineTotal { get; set; }

        /// <summary>
        /// Navigation property to the purchase invoice
        /// </summary>
        [ForeignKey("PurchaseID")]
        public virtual PurchaseInvoice Purchase { get; set; } = null!;

        /// <summary>
        /// Navigation property to the material
        /// </summary>
        [ForeignKey("MaterialID")]
        public virtual Material Material { get; set; } = null!;
    }
}

